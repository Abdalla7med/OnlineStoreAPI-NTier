using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineStoreAPI;

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

        public async Task AddAsync(ProductCreateDto product)
        {
            Product Product = new()
            {
                Name = product.Name,
                Price = product.Price,
                QuantityInStock = product.QuantityInStock
            };

            await repository.InsertAsync(Product);
            await repository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await repository.GetByIdAsync(id);

            if(product == null)
            {
                throw new Exception("Not Found");
            }
            
            repository.Delete(product);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var Entities = await repository.GetByIdAsync(id);

            return Entities;
        }

        public async Task UpdateAsync(int id, ProductUpdateDto updatedProduct)
        {
            var product = await repository.GetByIdAsync(id);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            // Update the product properties
            product.QuantityInStock = updatedProduct.QuantityInStock;
            product.Price = updatedProduct.Price;

            // Save changes
            repository.Update(product);
            await repository.SaveAsync();
        }
    }
}
