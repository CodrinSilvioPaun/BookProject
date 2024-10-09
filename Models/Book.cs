using BookProject.Events;

namespace BookProject.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Author> Authors { get; set; }
        public DateTime? PublishedDate { get; set; }

        public Book()
        {
        }

        public Book(Guid id, string title, string description, List<Author> authors, DateTime? publishedDate)
        {
            Id = id;
            Title = title;
            Description = description;
            Authors = authors;
            PublishedDate = publishedDate;
        }

        public void Append(Event @event)
        {
            switch (@event)
            {
                case BookCreated bookCreated:
                    Append(bookCreated);
                    break;
                case BookUpdated bookUpdated:
                    Append(bookUpdated);
                    break;
            }
        }
        private void Append(BookCreated bookCreated)
        {
            Id = bookCreated.Id;
            Title = bookCreated.Title;
            Description = bookCreated.Description;
            Authors = bookCreated.Authors;
            PublishedDate = bookCreated.PublishedDate;
        }

        private void Append(BookUpdated bookUpdated)
        {
            if (!string.IsNullOrEmpty(bookUpdated.Title?.Trim()))
            {
                Title = bookUpdated.Title;
            }

            if (!string.IsNullOrEmpty(bookUpdated.Description?.Trim()))
            {
                Description = bookUpdated.Description;
            }

            if (bookUpdated.Authors != null)
            {
                Authors = bookUpdated.Authors;
            }

            if (bookUpdated.PublishedDate != null && bookUpdated.PublishedDate <= DateTime.Now)
            {
                PublishedDate = bookUpdated.PublishedDate;
            }

        }
    }
}
