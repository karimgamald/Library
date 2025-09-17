using Library.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public bool Status { get; set; }
        //public MainType Type { get; set; }
        //public int TotalBooks { get; set; }
        //public int TotalPenaltys { get; set; }
        public string? PhotoPath { get; set; }

        // Not mapped, only for upload
        [NotMapped]
        public IFormFile? Photo { get; set; }

        [ForeignKey("MemberShip")]
        public int? MemberShipId { get; set; }
        public MemberShip? MemberShip { get; set; }

        public ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();

    }
}
