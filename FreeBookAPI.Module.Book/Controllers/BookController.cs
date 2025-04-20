using FreeBookAPI.Application.DTO;
using FreeBookAPI.Application.Interfaces;
using FreeBookAPI.Infrastructure.StorageAPI;
using FreeBookAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.IO;
using System.Runtime.CompilerServices;
using static System.Reflection.Metadata.BlobBuilder;

namespace FreeBookAPI.Module.Book.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _service;
    private readonly IUserService _userService;
    public BookController(IBookService service, IUserService userService)
    {
        _service = service;
        _userService = userService;
    }
    [HttpPost("CreateBook")]
    public async Task<IActionResult> CreateBook(BookCreate bookCreate)
    {
        try
        {
            if(bookCreate == null)
            {
                return BadRequest("Нету данных для добавления книги");
            }
            var imageFile = bookCreate.Files.FirstOrDefault(f => f.ContentType.StartsWith("image/")) 
                ?? throw new Exception("Не хватает image файла");
            var pdfFile = bookCreate.Files.FirstOrDefault(f => f.ContentType == "application/pdf") 
                ?? throw new ArgumentNullException("Не хватает pdf файла");

            if (imageFile == null || pdfFile == null)
            {
                return BadRequest("Необходимы и изображение, и PDF-файл.");
            }

            await _service.CreateBook(bookCreate.BookDTO, new[] { imageFile, pdfFile });

            return Created();
        }
        catch (Exception ex)
        {
            return BadRequest($"Произошла ошибка сервера - {ex.Message}");
        }
    }

    [HttpGet("GetBooks")]
    public async Task<IActionResult> GetBooks([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string? search = null)
    {
        try
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Текущая страница или размер страницы не может быть 0 или меньше");
            }

            var (books, totalItems) = await _service.GetBookCovers(page, pageSize, search);
            var pagedBooksResult = new PagedBooksResult
            {
                Books = books,
                TotalItems = totalItems
            };

            return Ok(pagedBooksResult);
        }
        catch (Exception ex)
        {
            return BadRequest($"Произошла ошибка сервера - {ex.Message}");
        }
    }
    [HttpGet("DownloadBook/{id:guid}")]
    public async Task<IActionResult> DownloadBook(Guid id)
    {
        try
        {
            var bookFile = await _service.DownloadBook(id);
            return File(bookFile.Content, bookFile.ContentType, bookFile.FileName);
        }
        catch (Exception ex)
        {
            return BadRequest($"Произошла ошибка сервера - {ex.Message}");
        }
    }

    [HttpPost("DeleteBook/{id:guid}")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        try
        {
            await _service.RemoveBook(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest($"Произошла ошибка сервера - {ex.Message}");
        }
    }

    [HttpPost("DeleteBookSoft/{id:guid}")]
    public async Task<IActionResult> DeleteBookSoft(Guid id)
    {
        try
        {
            await _service.RemoveBookSoft(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest($"Произошла ошибка сервера - {ex.Message}");
        }
    }

    [HttpPost("CreateUser/{id:long}")]
    public async Task<IActionResult> CreateUser(long id)
    {
        try
        {
            var user = new User
            {
                TelegramUserId = id,
            };
            await _userService.CreateUser(user);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest($"Произошла ошибка сервера - {ex.Message}");
        }
    }

    [HttpGet("GetUser/{id:long}")]
    public async Task<IActionResult> GetUser(long id)
    {
        try
        {
            return Ok(await _userService.GetUser(id));
        }
        catch (Exception ex)
        {
            return BadRequest($"Произошла ошибка сервера - {ex.Message}");
        }
    }

    [HttpPost("UpdateUserPage/{id:long}/{newPage:int}/{bookId:guid}")]
    public async Task<IActionResult> UpdateUserPage(long id, int newPage, Guid bookId)
    {
        try
        {
            
            await _userService.UpdateBookPageForUser(id,newPage,bookId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest($"Произошла ошибка сервера - {ex.Message}");
        }
    }

    [HttpGet("GetCurrentBookPage/{id:long}/{bookId:guid}")]
    public async Task<IActionResult> GetCurrentBookPage(long id, Guid bookId)
    {
        try
        {
            return Ok(await _userService.GetCurrentBookPage(id, bookId));
        }
        catch (Exception ex)
        {
            return BadRequest($"Произошла ошибка сервера - {ex.Message}");
        }
    }

    [HttpPost("AddFavoriteBook/{id:long}/{bookId:guid}")]
    public async Task<IActionResult> AddFavoriteBook(long id, Guid bookId)
    {
        try
        {
            await _userService.AddFavoriteBook(id, bookId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest($"Произошла ошибка сервера - {ex.Message}");
        }
    }

    [HttpGet("GetFavoriteBooks/{id:long}")]
    public async Task<IActionResult> GetFavoriteBooks(long id)
    {
        try
        {
            return Ok(await _userService.GetFavoriteBooks(id));
        }
        catch (Exception ex)
        {
            return BadRequest($"Произошла ошибка сервера - {ex.Message}");
        }
    }

    [HttpGet("GetFavoriteBooksUser")]
    public async Task<IActionResult> GetFavoriteBooksUser([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] long telegramId)
    {
        try
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Текущая страница или размер страницы не может быть 0 или меньше");
            }

            var (books, totalItems) = await _service.GetFavoriteBooks(telegramId, page, pageSize);

            var pagedBooksResult = new PagedBooksResult
            {
                Books = books,
                TotalItems = totalItems
            };

            return Ok(pagedBooksResult);

        }
        catch (Exception ex)
        {
            return BadRequest($"Произошла ошибка сервера - {ex.Message}");
        }
    }


}
