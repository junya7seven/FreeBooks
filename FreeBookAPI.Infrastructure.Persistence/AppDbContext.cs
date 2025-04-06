using Microsoft.EntityFrameworkCore;
using FreeBookAPI.Models;

namespace FreeBookAPI.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; } = null;
        public DbSet<BookPDF> BookPDFs { get; set; } = null;
        public DbSet<BookImage> BookImages { get; set; } = null;


        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(b => b.BookImage)
                .WithOne(bi => bi.Book)
                .HasForeignKey<BookImage>(bi => bi.BookId) 
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Book>()
                .HasOne(b => b.BookPDF)
                .WithOne(bp => bp.Book)
                .HasForeignKey<BookPDF>(bp => bp.BookId) 
                .OnDelete(DeleteBehavior.Cascade); 
        }

    }
}
