using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeBookAPI.Models;

namespace FreeBookAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CheckExistUser(long telegramId);
        Task CreateUser(User user);
        Task<User?> GetUser(long userId);
        Task<int> GetCurrentBookPage(long userId,Guid bookId);
        Task UpdateBookPageForUser(long id, int newPage, Guid bookId);
        Task AddFavoriteBook(FavoriteBook favoriteBook);
        Task DeleteFavoriteBook(FavoriteBook favoriteBook);
        Task<FavoriteBook?> GetFavofiteBook(FavoriteBook favoriteBook);
        Task<IEnumerable<FavoriteBook?>> GetFavoriteBooks(Guid userId);
    }
}
