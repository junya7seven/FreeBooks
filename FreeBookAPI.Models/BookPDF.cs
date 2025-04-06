namespace FreeBookAPI.Models
{
    public class BookPDF
    {
        public Guid BookPDFId { get; set; }
        public string PdfPath { get; set; }
        public Guid BookId { get; set; }
        public Book Book { get; set; }
    }
}
