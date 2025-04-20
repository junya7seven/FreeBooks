using FreeBookAPI.Infrastructure.Persistence;
using FreeBookAPI.Interfaces;
using FreeBookAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBook.Infrastructure.Repositories.Service
{
    public class BookQuery : IBookQuery
    {
        private readonly AppDbContext _context;

        public BookQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Book> GetBookById(Guid id)
        {
            return await _context.Books
                .Include(b => b.BookImage)
                .Include(b => b.BookPDF)
                .FirstOrDefaultAsync(b => b.BookId == id);
        }

        public async Task<IEnumerable<Book>?> GetBookByAuthor(string authorName)
        {
            var t = new List<Book>();
            return t;
        }
        public async Task<(IEnumerable<Book> books, int totalItems)> GetPageBooks(int currentPage, int pageSize, string? search)
        {
            var query = _context.Books
                .Where(b => !b.isRevoke && (string.IsNullOrEmpty(search) || b.Category.Contains(search)));

            int totalItems = await query.CountAsync();

            var books = await query
                .Include(b => b.BookPDF)
                .Include(b => b.BookImage)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (books, totalItems);
        }
        public async Task<(IEnumerable<Book> books, int totalItems)> GetAllBooksById(int currentPage, int pageSize, params Guid[] bookId)
        {
            var query = _context.Books
                .Where(b => bookId.Contains(b.BookId));

            int totalItems = await query.CountAsync();

            var books = await query
                .Include(b => b.BookPDF)
                .Include(b => b.BookImage)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (books, totalItems);
        }

    }
}
