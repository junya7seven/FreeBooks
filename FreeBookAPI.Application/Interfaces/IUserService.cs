using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeBookAPI.Models;

namespace FreeBookAPI.Application.Interfaces
{
    public interface IUserService
    {
        Task CreateUser(User user);
        Task<User> GetUser(long userId);
        Task UpdateBookPageForUser(long id, int newPage, Guid bookId);
        Task<int> GetCurrentBookPage(long id, Guid bookId);
        Task AddFavoriteBook(long id, Guid bookId);
        Task<IEnumerable<FavoriteBook?>> GetFavoriteBooks(long id);
    }
}
