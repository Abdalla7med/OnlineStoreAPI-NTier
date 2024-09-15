using DAL;
using Microsoft.EntityFrameworkCore;

namespace BLL
{
    public class OrderServiceBase
    {

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            var orders = await repository.GetAllAsync(
                     o => o.Customer,
                     o => o.OrderDetails,
                     o => o.OrderDetails.ThenInclude(od => od.Product)
                 );

            return orders;
        }
    }
}