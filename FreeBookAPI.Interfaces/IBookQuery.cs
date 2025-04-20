using FreeBookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBookAPI.Interfaces
{
    public interface IBookQuery
    {
        Task<Book?> GetBookById(Guid id);
        Task<(IEnumerable<Book> books, int totalItems)> GetAllBooksById(int currentPage, int pageSize, params Guid[] bookId);
        Task<IEnumerable<Book>?> GetBookByAuthor(string authorName);
        Task<(IEnumerable<Book> books, int totalItems)> GetPageBooks(int currentPage, int pageSize, string? search);

    }
}
