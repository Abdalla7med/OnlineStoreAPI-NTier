using DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T> >GetAllAsync() => await _dbSet.ToListAsync();
        public async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task InsertAsync(T entity) => await _dbSet.AddAsync(entity);
        public async Task Update(T entity) => _dbSet.Update(entity);
        public async Task Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id);
             _dbSet.Remove(entity);
        }


        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
