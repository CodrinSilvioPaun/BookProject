using BookProject.Contracts.Book.Queries;
using BookProject.Database;
using BookProject.Events;
using BookProject.Models;

namespace BookProject.Services
{
    public class BookService : IBookService
    {
        private static readonly BookDatabase BookDatabase = new();
        public void CreateBook(Book book)
        {
            BookCreated bookCreated = new BookCreated
            {
                BookId = book.Id,
                Title = book.Title,
                Description = book.Description,
                Authors = book.Authors,
                PublishedDate = book.PublishedDate
            };

            BookDatabase.Append(bookCreated);
        }

        public void UpdateBook(Book book)
        {
            BookUpdated bookUpdate = new BookUpdated
            {
                BookId = book.Id,
                Title = book.Title,
                Description = book.Description,
                Authors = book.Authors,
                PublishedDate = book.PublishedDate
            };

            BookDatabase.Append(bookUpdate);
        }

        public Book GetBook(Guid id)
        {
            return BookDatabase.GetBook(id);
        }

        public IEnumerable<KeyValuePair<DateTime, Event>> GetBookHistory(BookHistorySearchQuery query)
        {
            return BookDatabase.GetBookHistory(query);
        }
    }
}
