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
    }
}
