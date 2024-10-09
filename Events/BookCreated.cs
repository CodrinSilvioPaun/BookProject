using BookProject.Models;

namespace BookProject.Events
{
    public class BookCreated : Event
    {
        public Guid BookId { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public List<Author> Authors { get; init; }
        public DateTime? PublishedDate { get; init; }
        public override Guid Id => BookId;
    }
}
