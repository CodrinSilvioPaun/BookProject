using BookProject.Contracts.Book.Queries;
using BookProject.Events;
using BookProject.Models;

namespace BookProject.Services
{
    public interface IBookService
    {
        public void CreateBook(Book book);

        public void UpdateBook(Book book);

        public Book GetBook(Guid id);

        public IEnumerable<KeyValuePair<DateTime, Event>> GetBookHistory(BookHistorySearchQuery query);
    }
}
