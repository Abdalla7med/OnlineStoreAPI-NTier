using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CustomerService
    {
        private readonly CustomerRepository repository;

        public CustomerService(CustomerRepository repository)
        {
            this.repository = repository;
        }

        public async Task AddAsync(Customer customer)
        {
            await repository.InsertAsync(customer);
        }

        public async Task DeleteAsync(int id)
        {
            await repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            var Entities = await repository.GetByIdAsync(id);
            return Entities;
        }

        public async Task UpdateAsync(Customer customer)
        {
            Customer old = await repository.GetByIdAsync(customer.Id);
            if (this.Equals(old))
                return;
            await repository.UpdateAsync(customer);
        }
    }
}
