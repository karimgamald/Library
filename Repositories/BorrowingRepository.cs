using Library.Data;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public class BorrowingRepository : IBorrowingRepository
    {
        private readonly LibraryDbContext _context;
        public BorrowingRepository(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Borrowing borrowing)
        {
            await _context.Borrowings.AddAsync(borrowing);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Borrowing borrowing)
        {
            _context!.Remove(borrowing);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Borrowing>> GetAllAsync()
            => await _context.Borrowings
                .Include(b => b.User)
                .ThenInclude(u => u.MemberShip)
                .Include(b => b.Book)
                .ToListAsync();

        public async Task<Borrowing> GetByIdAsync(int id)
            => (await _context.Borrowings
                .Include(b => b.User)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(m => m.Id == id))!;

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<Borrowing> borrowings)
        {
            _context.Borrowings.UpdateRange(borrowings);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Borrowing borrowing)
        {
            _context.Borrowings.Update(borrowing);
            await _context.SaveChangesAsync();
        }
    }
}
