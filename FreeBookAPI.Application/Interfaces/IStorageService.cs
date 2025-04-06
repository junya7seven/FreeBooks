using FreeBookAPI.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBookAPI.Application.Interfaces
{
    public interface IStorageService
    {
        Task<bool> UploadFile(BookFile fileModel);
        Task<BookFile> DownloadFile(Guid id);
        Task<string?> GetImagePath(Guid id);
        Task DeleteFile(Guid id);
    }
}
