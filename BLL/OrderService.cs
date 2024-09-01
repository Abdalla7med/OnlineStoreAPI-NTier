using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
namespace BLL
{
    /// <summary>
    ///  Business Logic About Order Operations must be here 
    /// </summary>
    public class OrderService
    {

        private readonly OrderRepository repository;
        public OrderService(OrderRepository repo)
        {
            repository = repo;
        }

        public async Task AddAsync(Order order)
        {
           await repository.InsertAsync(order);
        }

        public async Task DeleteAsync(int id)
        {
            await repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            var Entities = await repository.GetByIdAsync(id);
            return Entities;
        }

        public async Task UpdateAsync(Order order)
        {
            await repository.UpdateAsync(order);
        }
    }
}
