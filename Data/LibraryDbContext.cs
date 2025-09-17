using Library.Enums;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Borrowing> Borrowings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Fiction" },
            new Category { Id = 2, Name = "History" },
            new Category { Id = 3, Name = "Science" }
            );

            modelBuilder.Entity<MemberShip>().HasData(
                new
                {
                    Id = 1,
                    FinePerDay =3m,
                    ExtraBooks = 3,
                    ExtraDays = 14,
                    ExtraPenaltys = 15.00m,
                    MemberShipType = MemberShipType.Normal
                },
                new
                {
                    Id = 2,
                    FinePerDay = 2m,
                    ExtraBooks = 5,
                    ExtraDays = 21,
                    ExtraPenaltys = 10.00m,
                    MemberShipType = MemberShipType.Premium
                },
                new
                {
                    Id = 3,
                    FinePerDay = 1m,
                    ExtraBooks = 10,
                    ExtraDays = 30,
                    ExtraPenaltys = 5.00m,
                    MemberShipType = MemberShipType.Vip
                }
            );
        }
    }
}
