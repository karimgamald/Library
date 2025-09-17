using Library.Enums;

namespace Library.Models
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int ISBN { get; set; }
        public DateOnly PublishYear { get; set; }
        public int AvailableCopies { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public BookStatus Status { get; set; }
    }
}
