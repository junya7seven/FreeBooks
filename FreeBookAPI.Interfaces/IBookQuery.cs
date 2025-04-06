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
        Task<IEnumerable<Book>?> GetBookByAuthor(string authorName);
        Task<(IEnumerable<Book>, int)> GetPageBooks(int currentPage, int pageSize);

    }
}
