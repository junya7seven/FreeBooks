using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBookAPI.Models
{
    public class User
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string AboutUser { get; set; }
        public string PhoneNumber { get; set; }
    }
}
