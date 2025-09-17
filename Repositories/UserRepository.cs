using Library.Data;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LibraryDbContext _context;
        public UserRepository(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context!.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
            => await _context.Users
            .Include(u => u.MemberShip)
            .ToListAsync();

        public async Task<User> GetByIdAsync(int id)
            => (await _context.Users
                .Include(u => u.Borrowings)
                .ThenInclude(b => b.Book)
                .Include(u => u.MemberShip)
                .FirstOrDefaultAsync(u => u.Id == id))!;

        public async Task<User> GetByUserAndPassAsync(string username, string password)
            => (_context.Users
            .FirstOrDefault(u => u.UserName == username && u.Password == password))!;
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}