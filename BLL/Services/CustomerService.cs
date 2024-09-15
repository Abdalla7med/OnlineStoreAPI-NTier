using DAL;
using OnlineStoreAPI;
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

        public async Task<int> AddAsync(CustomerCreateDTO CustomerDto)
        {
            Customer customer = new()
            {
                Name = CustomerDto.Name,
                Email = CustomerDto.Email,
                PhoneNumber = CustomerDto.PhoneNumber,
                Orders = new List<Order>()
            };

            await repository.InsertAsync(customer);
            await repository.SaveAsync();

            return customer.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await repository.GetByIdAsync(id);
            if (customer == null)
                throw new Exception("Customer Not Found");

            repository.Delete(customer);
            await repository.SaveAsync();
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            var customer = await repository.GetByIdAsync(id);

            if (customer == null)
                throw new Exception("Customer Not Found");

            return customer;
        }

        public async Task<IEnumerable<Customer>> GetByNameAsync(string Name)
        {
            try
            {
                var Customers = await repository.GetAllAsync();

                var CustomerByName = Customers.Where(C => C.Name == Name);

                return CustomerByName;
            }
            catch(Exception ex)
            {
                throw (ex);
            }
        }


        public async Task UpdateAsync(int id, CustomerUpdateDTO customerDto)
        {
            var customer =  await  repository.GetByIdAsync(id);

            if (customer == null)
                throw new Exception("Customer not Found");

            /// updating Required Fields
            customer.PhoneNumber = customerDto.PhoneNumber;
            customer.Email = customerDto.Email;

            /// Reflect Changes back into database 
            repository.Update(customer);
            await repository.SaveAsync();

        }
    }
}
