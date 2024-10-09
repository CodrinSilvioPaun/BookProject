using BookProject.Models;

namespace BookProject.Contracts.Book.Responses
{
    public record BookResponse
    (
        Guid BookId,
        string Title,
        string Description,
        List<Author> Authors,
        DateTime? PublishedDate
    );
}
