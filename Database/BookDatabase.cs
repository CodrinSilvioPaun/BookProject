using BookProject.Contracts.Book.Queries;
using BookProject.Events;
using BookProject.Models;
using System.Collections.Concurrent;

namespace BookProject.Database
{
    public class BookDatabase
    {   // Used two in-memory data structures to keep track of all the events occurred and to have the latest version of the Book entity
        private readonly ConcurrentDictionary<Guid, SortedList<DateTime, Event>> _bookEvents = new();
        private readonly ConcurrentDictionary<Guid, Book> _bookView = new();

        public void Append(Event @event)
        {
            var stream = _bookEvents!.GetValueOrDefault(@event.Id, null);

            switch (@event)
            {
                case BookCreated bookCreated:
                    Create(bookCreated, stream);
                    break;
                case BookUpdated bookUpdated:
                    Update(bookUpdated, stream);
                    break;
                default:
                    throw new InvalidOperationException($"The event {@event} doesn't exist");
            }
        }

        private void Update(BookUpdated bookUpdated, SortedList<DateTime, Event> stream)
        {
            ValidateBookUpdate(bookUpdated, stream);
            UpdateBookProjection(bookUpdated, _bookView[bookUpdated.Id]);
        }

        private void Create(BookCreated bookCreated, SortedList<DateTime, Event>? stream)
        {
            if (stream is null)
            {
                _bookEvents[bookCreated.Id] = new SortedList<DateTime, Event> { { bookCreated.DateCreated, bookCreated } };
                UpdateBookProjection(bookCreated, new Book());
            }
        }

        // Used a projection to maintain a good performance while retrieving a Book and Updating  Book's fields
        private void UpdateBookProjection(Event @event, Book book)
        {
            book.Append(@event);
            _bookView.AddOrUpdate(book.Id, book, (key, oldValue) => book);
        }

        public Book GetBook(Guid id)
        {
            if (!_bookView.ContainsKey(id))
            {
                throw new KeyNotFoundException($"The key {id} was not found in the library");
            }
            return _bookView[id];
        }
        // Pagination wasn't added because we already have an in-memory dictionary, with a real database I would have added
        // the pageNumber and pageSize to BookHistorySearchQuery and then use LINQ by applying: Skip((pageIndex - 1) * pageSize).Take(pageSize);
        public IEnumerable<KeyValuePair<DateTime, Event>> GetBookHistory(BookHistorySearchQuery query)
        {
            // Filtering implemented on the Title and Description fields
            IEnumerable<KeyValuePair<DateTime, Event>> result;
            if (!string.IsNullOrEmpty(query.Title) && !string.IsNullOrEmpty(query.Description))
            {
                result = _bookEvents[query.Id].Where(x =>
                    x.Value is BookUpdated bookUpd && (!string.IsNullOrEmpty(bookUpd.Title) && bookUpd.Title.Contains(query.Title, StringComparison.OrdinalIgnoreCase)) &&
                    (!string.IsNullOrEmpty(bookUpd.Description) && bookUpd.Description.Contains(query.Description, StringComparison.OrdinalIgnoreCase)));
            }
            else if (!string.IsNullOrEmpty(query.Title))
            {
                result = _bookEvents[query.Id].Where(x =>
                    x.Value is BookUpdated bookUpd && (!string.IsNullOrEmpty(bookUpd.Title) && bookUpd.Title.Contains(query.Title, StringComparison.OrdinalIgnoreCase)));
            }
            else if (!string.IsNullOrEmpty(query.Description))
            {
                result = _bookEvents[query.Id].Where(x =>
                    x.Value is BookUpdated bookUpd && (!string.IsNullOrEmpty(bookUpd.Description) && bookUpd.Description.Contains(query.Description, StringComparison.OrdinalIgnoreCase)));
            }
            else
            {
                result = _bookEvents[query.Id].Where(x => x.Value is BookUpdated);
            }
            // Ordering implemented on the date in which the event was added
            if (query.OrderByDateAddedDesc)
            {
                return result.OrderByDescending(x => x.Key);
            }
            return result;
        }

        private void ValidateBookUpdate(BookUpdated bookUpdated, SortedList<DateTime, Event> stream)
        {
            if (stream is null)
            {
                throw new KeyNotFoundException($"The key {bookUpdated.Id} was not found in the library");
            }

            if (bookUpdated.Title != null && bookUpdated.Title.Equals(_bookView[bookUpdated.Id].Title))
            {
                throw new InvalidOperationException(
                    $"The {nameof(bookUpdated.Title)} field already has the value meant to be updated");
            }

            if (bookUpdated.Description != null && bookUpdated.Description.Equals(_bookView[bookUpdated.Id].Description))
            {
                throw new InvalidOperationException(
                    $"The {nameof(bookUpdated.Description)} field already has the value meant to be updated");
            }

            if (bookUpdated.Authors != null &&
                bookUpdated.Authors.Any(x => _bookView[bookUpdated.Id].Authors.Any(y => x.Id == y.Id)))
            {
                throw new InvalidOperationException(
                    $"The {nameof(bookUpdated.Authors)} field already contains a value meant to be updated");
            }

            if (bookUpdated.PublishedDate != null && bookUpdated.PublishedDate.Equals(_bookView[bookUpdated.Id].PublishedDate))
            {
                throw new InvalidOperationException(
                    $"The {nameof(bookUpdated.PublishedDate)} field already has the value meant to be updated");
            }

            if (!_bookEvents[bookUpdated.Id].TryAdd(bookUpdated.DateCreated, bookUpdated))
            {
                throw new InvalidOperationException("The same operation is already being done by another API");
            }
        }
    }
}
