using Library.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string  Author { get; set; } = string.Empty;
        public int ISBN { get; set; }
        public DateOnly PublishYear { get; set; }
        public int AvailableCopies { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        public string? ImageUrl {  get; set; }
        public BookStatus Status { get; set; }

        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
