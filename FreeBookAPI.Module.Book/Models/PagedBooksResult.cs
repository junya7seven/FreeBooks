using FreeBookAPI.Application.DTO;
using FreeBookAPI.Models;
public class PagedBooksResult
{
    public IEnumerable<BookDTO> Books { get; set; }
    public int TotalItems { get; set; }
}
