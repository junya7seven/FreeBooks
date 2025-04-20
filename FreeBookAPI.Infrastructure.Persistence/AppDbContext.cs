using Microsoft.EntityFrameworkCore;
using FreeBookAPI.Models;

namespace FreeBookAPI.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; } = null;
        public DbSet<BookPDF> BookPDFs { get; set; } = null;
        public DbSet<BookImage> BookImages { get; set; } = null;
        public DbSet<User> Users { get; set; } = null;
        public DbSet<BookCurrentPage> CurrentPages { get; set; } = null;
        public DbSet<FavoriteBook> FavoriteBooks { get; set; } = null;

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

            modelBuilder.Entity<User>()
            .HasMany(u => u.BookCurrentPages)
            .WithOne(bcp => bcp.User)                
            .HasForeignKey(bcp => bcp.UserId)        
            .OnDelete(DeleteBehavior.Cascade);      

            modelBuilder.Entity<BookCurrentPage>()
                .HasKey(bcp => bcp.BookCurrentPageId);   

            modelBuilder.Entity<BookCurrentPage>()
                .HasOne(bcp => bcp.User)                
                .WithMany(u => u.BookCurrentPages)      
                .HasForeignKey(bcp => bcp.UserId)         
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
            .HasMany(u => u.FavoriteBooks)
            .WithOne(bcp => bcp.User)
            .HasForeignKey(bcp => bcp.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<FavoriteBook>()
                .HasKey(bcp => bcp.FavoriteBookId);

            modelBuilder.Entity<FavoriteBook>()
                .HasOne(bcp => bcp.User)
                .WithMany(u => u.FavoriteBooks)
                .HasForeignKey(bcp => bcp.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
