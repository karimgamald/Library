using Library.Models;

namespace Library.Interfaces
{
    public interface IMemberShipRepository
    {
        Task<IEnumerable<MemberShip>> GetAllAsync();
        Task<MemberShip> GetByIdAsync(int id);
        Task AddAsync(MemberShip memberShip);
        Task UpdateAsync(MemberShip memberShip);
        Task DeleteAsync(MemberShip memberShip);
        Task SaveChangesAsync();
    }
}
