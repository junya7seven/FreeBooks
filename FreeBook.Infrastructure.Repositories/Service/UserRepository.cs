using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeBookAPI.Infrastructure.Persistence;
using FreeBookAPI.Interfaces;
using FreeBookAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FreeBook.Infrastructure.Repositories.Service
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> CheckExistUser(long telegramId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.TelegramUserId == telegramId);
        }

        public async Task CreateUser(User user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUser(long telegramId)
        {
            return await _context.Users
                .Include(b => b.BookCurrentPages)
                .FirstOrDefaultAsync(u => u.TelegramUserId == telegramId);
        }

        public async Task<IEnumerable<FavoriteBook?>> GetFavoriteBooks(Guid userId)
        {
            return await _context.FavoriteBooks.Where(u => u.UserId == userId)
                .ToListAsync();
        }

        public async Task<int> GetCurrentBookPage(long id, Guid bookId)
        {
            var user = await _context.Users
            .Include(u => u.BookCurrentPages)
            .Where(u => u.TelegramUserId == id)
            .Where(u => u.BookCurrentPages.Any(b => b.BookId == bookId))
            .FirstOrDefaultAsync();

            var currentPage = user?.BookCurrentPages
            .FirstOrDefault(b => b.BookId == bookId)?
            .CurrentPage;

            return currentPage ?? 1;
        }

        public async Task UpdateBookPageForUser(long id, int newPage, Guid bookId)
        {
            var existUser = await _context.Users
                .Include(b => b.BookCurrentPages)
                .FirstOrDefaultAsync(u => u.TelegramUserId == id);
            var existBook = await _context.Books.AnyAsync(b => b.BookId == bookId);
            if (existUser != null && existBook)
            {
                var bookPage = existUser.BookCurrentPages.FirstOrDefault(b => b.BookId == bookId);
                if(bookPage != null)
                {
                    bookPage.CurrentPage = newPage;
                }
                else
                {
                    existUser.BookCurrentPages.Add(new BookCurrentPage
                    {
                        BookId = bookId,
                        CurrentPage = newPage,

                    });
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCurrentPage(long id, Guid bookId)
        {
            var user = await _context.Users
            .Include(u => u.BookCurrentPages)
            .Where(u => u.TelegramUserId == id)
            .Where(u => u.BookCurrentPages.Any(b => b.BookId == bookId))
            .FirstOrDefaultAsync();

            var currentPage = user?.BookCurrentPages
            .FirstOrDefault(b => b.BookId == bookId)?
            .CurrentPage;
        }

        public async Task AddFavoriteBook(FavoriteBook favoriteBook)
        {
                await _context.FavoriteBooks.AddAsync(favoriteBook);
                await _context.SaveChangesAsync();
        }
        public async Task DeleteFavoriteBook(FavoriteBook favoriteBook)
        {
            _context.FavoriteBooks.Remove(favoriteBook);
            await _context.SaveChangesAsync();
        }

        public async Task<FavoriteBook?> GetFavofiteBook(FavoriteBook favoriteBook)
        {
            return await _context.FavoriteBooks.FirstOrDefaultAsync(f => f.UserId == favoriteBook.UserId 
            && f.BookId == favoriteBook.BookId);
        }
    }
}
