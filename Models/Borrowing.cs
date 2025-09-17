using Library.Enums;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Borrowing
    {
        public int Id { get; set; }
        public DateTime BorrowDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; }
        public int TotalDays { get; set; }
        public DateTime? ReturnDate { get; set; }
        public BorrowingStatus Status { get; set; } = BorrowingStatus.Borrowed;
        public decimal TotalFines { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey("Book")]
        public int? BookId { get; set; }
        public Book? Book { get; set; }
    }
}
