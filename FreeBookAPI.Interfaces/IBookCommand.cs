using FreeBookAPI.Models;

namespace FreeBookAPI.Interfaces
{
    public interface IBookCommand
    {
        Task<bool> UpdateBook(Book book);
        Task<Book> CreateBook(Book book);
        Task<bool> RemoveBookSoft(Book book);
        Task<bool> RemoveBook(Book book);

    }
}
