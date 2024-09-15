using BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common;
using DAL;
namespace OnlineStoreAPI.Controllers
{
    [Route("api/[controller]/OrderManagement")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly OrderService _orderservice;

        /// <summary>
        /// Dependency Injection 
        /// </summary>
        /// <param name="OrderService"></param>
        public OrderController(OrderService OrderService ) 
        {
            this._orderservice = OrderService;
        }

        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderservice.GetAllAsync();
            if (orders != null && orders.Any())
            {
                // Customizing the response for each order
                var customizedOrders = orders.Select(order => new
                {
                    OrderId = order.Id,
                    OrderTransactionDate = order.OrderDate,
                    OrderStatus = order.Status,
                    OrderedBy = order.Customer.Name,
                    TotalPrice = order.OrderDetails.Sum(od => od.PriceAtPurchase)
                });

                return Ok(customizedOrders);
            }

            /// In case of no Orders available
            return NoContent();
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var Order = await _orderservice.GetByIdAsync(id);
            try
            {
                if (Order != null)
                {
                    /// Trying To Customize Date that will appears to the user 
                    return Ok(
                            new
                            {
                                OrderId = Order.Id,
                                OrderTransactionDate = Order.OrderDate,
                                OrderStatus = Order.Status,
                                OrderedBy = Order.Customer.Name,
                                TotalPrice = Order.OrderDetails.Sum(OD => OD.PriceAtPurchase)
                            });
                }
            }
            catch(Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
            /// InCase of No Orders Available
            return NoContent();
        }

        /// <summary>
        /// Note that the status code acting as chatting language btw developers
        /// </summary>
        /// <param name="OrderDto"></param>
        /// <returns></returns>
        [HttpPost("AddOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDTO OrderDto)
        {

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            /// Adding Order
            try
            {
               int id = await _orderservice.AddAsync(OrderDto);

                return CreatedAtAction(nameof(GetById), new { Id = id }, OrderDto);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpPut("UpdateOrder/{id:int}")]
        public async Task<IActionResult> UpdateOrder([FromRoute] int id, [FromBody] OrderUpdateDTO OrderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _orderservice.UpdateAsync(id, OrderDto);
                return Ok($"Order Status is Changed Successfully to {OrderDto.Status.ToString()}");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            try
            {
                await _orderservice.DeleteAsync(id);
                return Ok(value: "Order Deleted Successfully");
            }
            catch(Exception Ex)
            {
                return BadRequest(error:Ex.Message);
            }
        }
    }
}
