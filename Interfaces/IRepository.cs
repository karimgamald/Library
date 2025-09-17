using System.Linq.Expressions;

namespace Library.Interfaces
{
    public interface IRepository<T> where T : class
    {
        // Get all entities with optional filter, include and tracking
        Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            bool tracked = true);

        // Get single entity with optional filter, include and tracking
        Task<T?> GetAsync(
            Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            bool tracked = true);

        // Get entity by Id with include and tracking
        Task<T?> GetByIdAsync(
            int id,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            bool tracked = true);

        // Special case for login (Users only)
        Task<T?> GetByUserAndPassAsync(string username, string pass);

        // Basic CRUD
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);
        Task DeleteAsync(T entity);
    }
}
