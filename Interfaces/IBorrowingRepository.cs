using Library.Models;

namespace Library.Interfaces
{
    public interface IBorrowingRepository
    {
        Task<IEnumerable<Borrowing>> GetAllAsync();
        Task<Borrowing> GetByIdAsync(int id);
        Task AddAsync(Borrowing borrowing);
        Task UpdateAsync(Borrowing borrowing);
        Task UpdateRangeAsync(IEnumerable<Borrowing> borrowings);
        Task DeleteAsync(Borrowing borrowing);
        Task SaveChangesAsync();
    }
}
