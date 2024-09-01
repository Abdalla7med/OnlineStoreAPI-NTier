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
        public async Task InsertAsync(T Entity) => await _dbSet.AddAsync(Entity);
        public void Update(T Entity) => _dbSet.Update(Entity);
        public void Delete(T Entity) =>  _dbSet.Remove(Entity);
        
        public async Task SaveAsync() => await _context.SaveChangesAsync();
        
    }
}
