using BookProject.Models;

namespace BookProject.Contracts.Book.Requests
{
    public class UpdateBookRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<Author>? Authors { get; set; }
        public DateTime? PublishedDate { get; set; }
    }
}
