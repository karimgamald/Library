using Library.Data;
using Library.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Library.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly LibraryDbContext _context;
        internal DbSet<T> _dbSet;

        public Repository(LibraryDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            bool tracked = true)
        {
            IQueryable<T> query = tracked ? _dbSet : _dbSet.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            return await query.ToListAsync();
        }

        public async Task<T?> GetAsync(
            Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            bool tracked = true)
        {
            IQueryable<T> query = tracked ? _dbSet : _dbSet.AsNoTracking();

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task<T?> GetByIdAsync(
            int id,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            bool tracked = true)
        {
            IQueryable<T> query = tracked ? _dbSet : _dbSet.AsNoTracking();

            if (include != null)
                query = include(query);

            // assumes primary key is "Id"
            return await query.FirstOrDefaultAsync(e =>
                EF.Property<int>(e, "Id") == id);
        }

        public async Task<T?> GetByUserAndPassAsync(string username, string pass)
        {
            // only works if entity has UserName + Password
            return await _dbSet.FirstOrDefaultAsync(u =>
                EF.Property<string>(u, "UserName") == username &&
                EF.Property<string>(u, "Password") == pass);
        }

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public async Task UpdateAsync(T entity) => _dbSet.Update(entity);
        public async Task UpdateRangeAsync(IEnumerable<T> entities) => _dbSet.UpdateRange(entities);
        public async Task DeleteAsync(T entity) => _dbSet.Remove(entity);
    }
}
