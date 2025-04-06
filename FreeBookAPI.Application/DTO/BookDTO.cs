using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBookAPI.Application.DTO
{
    public class BookDTO
    {
        public string Category { get; set; } 
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public DateTime DateCreate { get; set; } 
        public string ImagePath { get; set; }
    }
}
