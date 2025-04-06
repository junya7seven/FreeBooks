using AutoMapper;
using FreeBookAPI.Application.DTO;
using FreeBookAPI.Application.Interfaces;
using FreeBookAPI.Interfaces;
using FreeBookAPI.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBookAPI.Application.Service
{
    public class BookService : IBookService
    {
        private readonly IBookCommand _bookCommand;
        private readonly IBookQuery _bookQuery;
        //private readonly IBookFileRepository _bookFileRepository;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly string BookPath;


        public BookService(IBookCommand bookCommand, 
            IBookQuery bookQuery, 
            //IBookFileRepository bookFileRepository, 
            IStorageService storageService,
            IMapper mapper,
            IConfiguration configuration)
        {
            _bookCommand = bookCommand;
            _bookQuery = bookQuery;
            //_bookFileRepository = bookFileRepository;
            _storageService = storageService;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить модель книг книга + bookImage
        /// </summary>
        /// <param name="page">текущая страница</param>
        /// <param name="pageSize">размер моделей на страницы</param>
        /// <returns>Коллекция bookDTO или null</returns>
        public async Task<IEnumerable<BookDTO?>> GetBookCovers(int page, int pageSize)
        {
            var (books,pages) = await _bookQuery.GetPageBooks(page, pageSize);
            return _mapper.Map<IEnumerable<BookDTO>>(books);
        }

        /// <summary>
        /// Создание книги
        /// </summary>
        /// <param name="bookDTO">модель книги</param>
        /// <param name="files">файлы для книги 1 - изображение, 2 - pdf файл</param>
        /// <returns></returns>
        public async Task CreateBook(BookDTO bookDTO, params BookFile[] files)
        {
            var book = _mapper.Map<Book>(bookDTO);
            files[0].Id = Guid.NewGuid();
            files[1].Id = Guid.NewGuid();

            book.BookImage = new BookImage
            {
                BookImageId = files[0].Id
            };

            book.BookPDF = new BookPDF
            {
                BookPDFId = files[1].Id
            };
            book.BookImageId = files[0].Id;
            book.BookPDFId = files[1].Id;
            try
            {
                var uploadTasks = new[]
            {
                _storageService.UploadFile(files[0]),
                _storageService.UploadFile(files[1])
            };

                var uploadResults = await Task.WhenAll(uploadTasks);

                if (uploadResults.Any(result => !result))
                {
                    throw new ArgumentException("Не удалось загрузить изображение или pdf файл");
                }
                /// для получения только что загруженного файла
                await Task.Delay(5000);
                book.BookImage.ImagePath = await _storageService.GetImagePath(files[0].Id);
                book.BookPDF.PdfPath = $"disk:/Books/{files[1].Id}";

                await _bookCommand.CreateBook(book);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Скачать pdf файл книги 
        /// </summary>
        /// <param name="id">id книги</param>
        /// <returns>файл</returns>
        public async Task<BookFile> DownloadBook(Guid id)
        {
            var existBook = await _bookQuery.GetBookById(id);
            if(existBook is not null)
            {
                var bookPdfId = existBook.BookPDF.BookPDFId;
                return await _storageService.DownloadFile(bookPdfId);
            }
            throw new ArgumentException("Такой книги не существует");
        }

        /// <summary>
        /// Мягкое удаление (isRevoke = true)
        /// </summary>
        /// <param name="id">id книги</param>
        /// <returns></returns>
        public async Task RemoveBookSoft(Guid id)
        {
            try
            {
                await _bookCommand.RemoveBookSoft(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось мягко удалить книгу");
            }
        }

        /// <summary>
        /// Полное удаление книги
        /// </summary>
        /// <param name="id">id книги</param>
        /// <returns></returns>
        public async Task RemoveBook(Guid id)
        {
            try
            {
                var existBook = await _bookQuery.GetBookById(id);

                if(existBook is null)
                {
                    throw new ArgumentException("Такой книги не существует");
                }

                var deleteFiles = new List<Task>();

                if (existBook.BookImage?.BookImageId != Guid.Empty)
                {
                    deleteFiles.Add(_storageService.DeleteFile(existBook.BookImage.BookImageId));
                }

                if (existBook.BookPDF?.BookPDFId != Guid.Empty)
                {
                    deleteFiles.Add(_storageService.DeleteFile(existBook.BookPDF.BookPDFId));
                }

                await Task.WhenAll(deleteFiles);

                await _bookCommand.RemoveBook(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
