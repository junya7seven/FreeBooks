using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBookAPI.Models
{
    public class BookImage
    {
        public Guid BookImageId { get; set; }
        public string? ImagePath { get; set; }
        public Guid BookId { get; set; }
        public Book Book { get; set; }
    }
}
