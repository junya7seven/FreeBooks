using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBookAPI.Models
{
    /// <summary>
    /// книга
    /// </summary>
    public class Book 
    {
        public Guid BookId { get; set; }
        public string? Category { get; set; } = "All";
        public string? Title { get; set; } = "No Content";
        public string? Description { get; set; } = "No Content";
        public string? AuthorName { get; set; } = "Unknown author";
        public string SuggestedBook { get; set; } = "Admin";
        public bool isRevoke { get; set; } = false;
        public DateTime DateCreate { get; set; } = DateTime.Now;


        public Guid BookImageId { get; set; }
        public Guid BookPDFId { get; set; }
        // связь с изображением
        public BookImage BookImage { get; set; }
        // связь с файлом
        public BookPDF BookPDF { get; set; }
    }
}
