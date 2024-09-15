using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRepository<T> where T : class, new()
    {
        abstract Task<IEnumerable<T>> GetAllAsync();
        abstract Task<T> GetByIdAsync(int id);
        Task InsertAsync(T Entity);
        void Update(T Entity);
        void Delete(T Entity);
        Task SaveAsync();
        
    }
}
