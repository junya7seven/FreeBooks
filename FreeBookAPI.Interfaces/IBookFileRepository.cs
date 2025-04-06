using FreeBookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBookAPI.Interfaces
{
    public interface IBookFileRepository
    {
        Task<bool> ChangePdfFile(Guid id, BookPDF bookPDF);
        Task<bool> ChangeImageFile(Guid id, BookImage bookImage);
    }
}
