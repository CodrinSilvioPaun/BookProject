using BookProject.Models;

namespace BookProject.Contracts.Book.Responses
{
    public record BookHistoryResponse
    (
        string Title,
        string Description,
        List<Author> Authors,
        DateTime? PublishedDate
    );
}
