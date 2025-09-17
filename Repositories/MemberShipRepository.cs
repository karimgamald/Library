using Library.Data;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Library.Repositories
{
    public class MemberShipRepository : IMemberShipRepository
    {
        private readonly LibraryDbContext _context;
        public MemberShipRepository(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(MemberShip memberShip)
        {
            await _context.MemberShips.AddAsync(memberShip);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(MemberShip memberShip)
        {
            _context!.Remove(memberShip);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MemberShip>> GetAllAsync()
            => await _context.MemberShips
            .Include(m => m.Users)
            .ToListAsync();

        public async Task<MemberShip> GetByIdAsync(int id)
            => (await _context.MemberShips.FindAsync(id))!;

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MemberShip memberShip)
        {
            _context.MemberShips.Update(memberShip);
            await _context.SaveChangesAsync();
        }
    }
}
