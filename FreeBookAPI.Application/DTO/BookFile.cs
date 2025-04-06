﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBookAPI.Application.DTO
{
    public class BookFile
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } 
        public string ContentType { get; set; } 
        public byte[] Content { get; set; }
    }
}
