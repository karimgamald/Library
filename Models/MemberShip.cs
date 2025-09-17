using Library.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class MemberShip
    {
        public int Id { get; set; }
        public MemberShipType MemberShipType { get; set; }
        public decimal FinePerDay { get; set; }
        public int ExtraBooks { get; set; }
        public int ExtraDays { get; set; }
        public decimal ExtraPenaltys { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}
