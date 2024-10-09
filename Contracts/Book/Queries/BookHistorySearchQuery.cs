namespace BookProject.Contracts.Book.Queries
{
    public class BookHistorySearchQuery
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool OrderByDateAddedDesc { get; set; }
    }
}
