using Library.Models;

namespace Library.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Book> BookRepository { get; }
        IRepository<User> UserRepository { get; }
        IRepository<Category> CategoryRepository { get; }
        IRepository<Borrowing> BorrowingRepository { get; }
        IRepository<MemberShip> MemberShipRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
