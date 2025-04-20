using FreeBookAPI.Application.DTO;
using FreeBookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBookAPI.Application.Interfaces
{
    public interface IBookService
    {
        /// <summary>
        /// Получить модель книг книга + bookImage
        /// </summary>
        /// <param name="page">текущая страница</param>
        /// <param name="pageSize">размер моделей на страницы</param>
        /// <returns>Коллекция bookDTO или null</returns>
        Task<(IEnumerable<BookDTO> books, int totalItems)> GetBookCovers(int page, int pageSize, string? search);

        /// <summary>
        /// Получить список понравившихся книг
        /// </summary>
        /// <param name="teleramId">телеграм ид</param>
        /// <param name="currentPage">текущая страница</param>
        /// <param name="pageSize">размер страницы</param>
        /// <returns>Коллекция bookDTO или null</returns>
        Task<(IEnumerable<BookDTO> books, int totalItems)> GetFavoriteBooks(long teleramId, int currentPage, int pageSize);

        /// <summary>
        /// Создание книги
        /// </summary>
        /// <param name="bookDTO">модель книги</param>
        /// <param name="files">файлы для книги 1 - изображение, 2 - pdf файл</param>
        /// <returns></returns>
        Task CreateBook(BookDTO bookDTO, params BookFile[] files);

        /// <summary>
        /// Скачать pdf файл книги 
        /// </summary>
        /// <param name="id">id книги</param>
        /// <returns>файл</returns>
        Task<BookFile> DownloadBook(Guid id);

        /// <summary>
        /// Мягкое удаление (isRevoke = true)
        /// </summary>
        /// <param name="id">id книги</param>
        /// <returns></returns>
        Task RemoveBook(Guid id);

        /// <summary>
        /// Полное удаление книги
        /// </summary>
        /// <param name="id">id книги</param>
        /// <returns></returns>
        Task RemoveBookSoft(Guid id);
    }
}
