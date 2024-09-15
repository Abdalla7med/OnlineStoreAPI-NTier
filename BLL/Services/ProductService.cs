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
        /// <summary>
        /// Adding Product, by mapping the Dto object to Product Object
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(ProductCreateDto product)
        {
            
            Product Product = new()
            {
                Name = product.Name,
                Price = product.Price,
                QuantityInStock = product.QuantityInStock
            };

            await repository.InsertAsync(Product);
            await repository.SaveAsync();

            return Product.Id;
        }

        /// <summary>
        ///  Delete Product, and Throwing Exception InCase of Any Violation Occurs 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task DeleteAsync(int id)
        {
            var product = await repository.GetByIdAsync(id);

            if(product == null)
            {
                throw new Exception("Not Found");
            }
            
            repository.Delete(product);
        }

        /// <summary>
        ///  Get All Products 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        /// <summary>
        ///  Get Product By Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Product> GetByIdAsync(int id)
        {
            var product = await repository.GetByIdAsync(id);

            return product;
        }


        public async Task<IEnumerable<Product>> GetByNameAsync(string Name)
        {
            IEnumerable<Product> Entities = await repository.GetAllAsync();

            Entities = Entities.Where(p => p.Name == Name);

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
