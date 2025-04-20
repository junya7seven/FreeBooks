using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBookAPI.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public long TelegramUserId { get; set; }
        public string Role { get; set; }

        public ICollection<BookCurrentPage> BookCurrentPages { get; set; } = new List<BookCurrentPage>();
        public ICollection<FavoriteBook> FavoriteBooks { get; set; } = new List<FavoriteBook>();

    }
}
