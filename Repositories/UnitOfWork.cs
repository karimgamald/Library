using Library.Data;
using Library.Interfaces;
using Library.Models;
using System.Diagnostics.Metrics;

namespace Library.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _context;
        public IRepository<Book> BookRepository { get; set; }

        public IRepository<User> UserRepository { get; private set; }

        public IRepository<Category> CategoryRepository { get; private set; }

        public IRepository<Borrowing> BorrowingRepository { get; private set; }

        public IRepository<MemberShip> MemberShipRepository { get; private set; }

        //Injection
        public UnitOfWork(LibraryDbContext context, IRepository<Book> bookRepository, IRepository<User> userRepository, IRepository<Category> categoryRepository, IRepository<Borrowing> borrowingRepository, IRepository<MemberShip> memberShipRepository)
        {
            _context = context;
            BookRepository = bookRepository;
            UserRepository = userRepository;
            CategoryRepository = categoryRepository;
            BorrowingRepository = borrowingRepository;
            MemberShipRepository = memberShipRepository;
        }

        public void Dispose()
        => _context.Dispose();

        public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();
    }
}
