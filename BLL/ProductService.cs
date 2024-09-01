using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// Business Logic of Product must add here 
    /// </summary>
    public class ProductService
    {

        private readonly ProductRepository repository;
        public ProductService(ProductRepository _repository) 
        {
            repository = _repository;
        }

        public async Task AddAsync(Product product)
        {
            await repository.InsertAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            await repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var Entities = await repository.GetByIdAsync(id);
            return Entities;
        }

        public async Task UpdateAsync(Product product)
        {
            Product old = await repository.GetByIdAsync(product.Id);
            if (this.Equals(old))
                return;
            await repository.UpdateAsync(product);
        }
    }
}
