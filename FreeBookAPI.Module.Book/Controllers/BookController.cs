using FreeBookAPI.Application.DTO;
using FreeBookAPI.Application.Interfaces;
using FreeBookAPI.Infrastructure.StorageAPI;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.IO;

namespace FreeBookAPI.Module.Book.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _service;
    public BookController(IBookService service)
    {
        _service = service;
    }

    [HttpPost("CreateBook")]
    public async Task<IActionResult> CreateBook(IFormFile pdfFile)
    {
        try
        {
            //var imageMemoryStream = new MemoryStream();
            var pdfMemoryStream = new MemoryStream();

            //await imageFile.CopyToAsync(imageMemoryStream);
            await pdfFile.CopyToAsync(pdfMemoryStream);

            var image = new BookFile
            {
                FileName = pdfFile.FileName,
                Content = pdfMemoryStream.ToArray(),
                ContentType = pdfFile.ContentType
            };

            var pdf = new BookFile
            {
                FileName = pdfFile.FileName,
                Content = pdfMemoryStream.ToArray(),
                ContentType = pdfFile.ContentType
            };

            BookFile[] files = new BookFile[2];
            files[0] = image;
            files[1] = pdf;

            var bookDto = new BookDTO
            {
                Category = "asdas",
                AuthorName = "name",
                Title = "asdas"
            };

            await _service.CreateBook(bookDto, files);
            return Created();
        }
        catch (Exception ex)
        {
            return BadRequest($"Произошла ошибка сервера - {ex.Message}");
        }

    }

    [HttpPost("GetBooks")]
    public async Task<IActionResult> GetBooks(int page, int pageSize)
    {
        try
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Текущая страница или размер страницы не может быть 0 или меньше");
            }
            return Ok(await _service.GetBookCovers(page, pageSize));
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

}
