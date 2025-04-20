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
    public class BookCommand : IBookCommand
    {
        private readonly AppDbContext _context;

        public BookCommand(AppDbContext context)
        {
            _context = context;
        }

        private async Task<Book> CheckExistBook(Guid id)
        {
            return await _context.Books.FirstOrDefaultAsync(x => x.BookId == id);
        }

        public async Task<bool> UpdateBook(Book book)
        {
            var existBook = await CheckExistBook(book.BookId);
            if (existBook is not null)
            {
                throw new ArgumentException("Такой книги не существует");
            }

            existBook.AuthorName = book.AuthorName ?? existBook.AuthorName;
            existBook.Title = book.Title ?? existBook.Title;
            existBook.Description = book.Description ?? existBook.Description;
            existBook.Category = book.Category ?? book.Category;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Book> CreateBook(Book book)
        {
            book.DateCreate = DateTime.Now;
            book.Category = book.Category.ToLower();
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<bool> RemoveBookSoft(Book book)
        {
            book.isRevoke = true;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveBook(Book book)
        {
            _context.Books.Remove(book);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
