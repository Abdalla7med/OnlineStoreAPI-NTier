using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Microsoft.EntityFrameworkCore;
using OnlineStoreAPI;
namespace BLL
{
    /// <summary>
    ///  Business Logic About Order Operations must be here 
    /// </summary>
    public class OrderService
    {

        private readonly OrderRepository repository;
        private readonly CustomerRepository customerRepository;
        private readonly ProductRepository productRepository;
        public OrderService(OrderRepository repo, CustomerRepository _customerRepository, ProductRepository _productRepository)
        {
            repository = repo;
            customerRepository = _customerRepository;
            productRepository = _productRepository;
        }

        public async Task AddAsync(OrderCreateDTO orderDto)
        {

            var customer = await customerRepository.GetByIdAsync(orderDto.CustomerId ?? 0);

            // Ensure that the customer who places the order exists
            if (customer == null)
            {
                throw new Exception("Customer not found.");
            }

            // Initialize a list to collect any errors related to stock availability
            var stockErrors = new List<string>();

            // Create the Order entity
            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                Status = (int)orderDto.Status,
                CustomerId = orderDto.CustomerId,
                Customer = customer
            };

            order.OrderDetails = new List<OrderDetail>();

            // Check if all products in the order have sufficient stock
            foreach (var detailDto in orderDto.OrderDetails)
            {
                var product = await productRepository.GetByIdAsync(detailDto.ProductId);

                if (product == null)
                {
                    stockErrors.Add($"Product with ID {detailDto.ProductId} not found.");
                    continue;
                }

                if (detailDto.Quantity > product.QuantityInStock)
                {

                    stockErrors.Add($"Insufficient stock for Product ID {detailDto.ProductId}. Available: {product.QuantityInStock}, Requested: {detailDto.Quantity}");
                    continue;
                }

                // If stock is sufficient, add to the order details
                var orderDetail = new OrderDetail
                {
                    ProductId = detailDto.ProductId,
                    Quantity = detailDto.Quantity,
                    PriceAtPurchase = product.Price * detailDto.Quantity,
                    Product = product
                };
                order.OrderDetails.Add(orderDetail);
            }

            // Return errors if any stock issues were found
            if (stockErrors.Any())
            {
                throw new Exception(string.Join("; ", stockErrors));
            }

            // Update stock quantities for valid order details
            foreach (var orderDetail in order.OrderDetails)
            {
                var product = await productRepository.GetByIdAsync(orderDetail.ProductId);
                if (product != null)
                {
                    product.QuantityInStock -= orderDetail.Quantity;
                     productRepository.Update(product);
                    await productRepository.SaveAsync();

                }
            }

            // Add the order to the database
            await repository.InsertAsync(order);
            await repository.SaveAsync();

        }

        public async Task DeleteAsync(int id)
        {

            var Order = await repository.GetByIdAsync(id);

            if (Order == null)
            {
                throw new Exception("Order not Found");
            }

            repository.Delete(Order);
            await repository.SaveAsync();
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

        public async Task UpdateAsync(int id, OrderUpdateDTO updateorder)
        {
            var Order = await repository.GetByIdAsync(id);

            if (Order == null)
            {
                throw new Exception("Order not Found");
            }

            Order.Status =(int)updateorder.Status;

            repository.Update(Order);
            await repository.SaveAsync();
        }
    }
}
