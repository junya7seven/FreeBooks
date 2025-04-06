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

        public async Task<Book?> GetBookById(Guid id)
        {
            return await _context.Books
                .Include(b => b.BookImage)
                .Include(b => b.BookPDF)
                .FirstOrDefaultAsync(x => x.BookId == id);
        }

        public async Task<IEnumerable<Book>?> GetBookByAuthor(string authorName)
        {
            var t = new List<Book>();
            return t;
        }
        public async Task<(IEnumerable<Book>, int)> GetPageBooks(int currentPage, int pageSize)
        {
            var books = await _context.Books
                .Where(b => b.isRevoke == false)
                .Include(b => b.BookPDF)
                .Include(b => b.BookImage)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            int countPages = await _context.Books
                .Where(b => b.isRevoke == false)
                .Include(b => b.BookImage)
                .CountAsync();
            return (books, countPages / pageSize);
        }
    }
}
