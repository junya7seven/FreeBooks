using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FreeBookAPI.Models
{
    public class FavoriteBook
    {
        public Guid FavoriteBookId { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }
    }
}
