using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBookAPI.Application.DTO
{
    public class BookCreate
    {
        public BookDTO BookDTO { get; set; }
        public List<BookFile> Files { get; set; }
    }
}
