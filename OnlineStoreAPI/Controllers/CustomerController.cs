using BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/[controller]/CustomerManagement")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _CustomerService;
        public CustomerController(CustomerService customerService)
        {
            this._CustomerService = customerService;
        }

        [HttpGet("GetAllCustomers")]
        public async Task<IActionResult> GetCustomers()
        {
            var Customers = await _CustomerService.GetAll();

            if(Customers!=null && Customers.Any())
            {
                var CustomizedCustomers = Customers.Select(C =>
                    new
                    {
                        CustomerID = C.Name,
                        ContactInfo = new { Phone = C.PhoneNumber, Email = C.Email },
                        NumberOfOrders = C.Orders.Count(),
                        TotalPurchasCost = C.Orders.Sum(O => O.OrderDetails.Sum(Od => Od.PriceAtPurchase)) /// Getting total Amount Customer Pay
                    });

                return Ok(CustomizedCustomers);
            }

            return NoContent();
        }

        [HttpGet("CustomerById/{id:int}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            try
            {
                var C = await _CustomerService.GetByIdAsync(id);

                if (C != null)
                {
                    var Customer = new
                    {
                        CustomerID = C.Name,
                        ContactInfo = new { Phone = C.PhoneNumber, Email = C.Email },
                        NumberOfOrders = C.Orders.Count(),
                        TotalPurchasCost = C.Orders.Sum(O => O.OrderDetails.Sum(Od => Od.PriceAtPurchase)) /// Getting total Amount Customer Pay
                    };

                    return Ok(Customer);
                }
            }
            catch(Exception Ex)
            {
                return BadRequest(Ex.Message);
            }

            /// Incase of No Exception, and No Customers Exists
            return NoContent();
        }

        [HttpGet("CustomerByName/{Name:alpha}")]
        public async Task<IActionResult> GetByName([FromRoute] string Name)
        {
            try
            {
                var Customers = _CustomerService.GetByNameAsync(Name);
                return Ok(Customers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
          
        }

        [HttpPost("AddingCustomer")]
        public async Task<IActionResult> AddCustomer(CustomerCreateDTO customerCreateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                int id =await _CustomerService.AddAsync(customerCreateDTO);

                return CreatedAtAction(nameof(GetById), new { Id = id }, customerCreateDTO);
            }
            catch(Exception Ex)
            {
                return BadRequest(Ex.Message);

            }
        }
        [HttpPut("UpdateCustomerInfo/{id:int}")]
        public async Task<IActionResult> UpdateCustomer([FromRoute] int id, [FromBody] CustomerUpdateDTO customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                await _CustomerService.UpdateAsync(id, customerDto);
                return Ok(new { Message = "Customer information updated successfully", CustomerId = id });
            }
            catch (Exception Ex)
            {
                return BadRequest($"An Error Occurred {Ex.Message}");
            }

        }

        [HttpDelete("DeleteCustomer/{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] int id)
        {
            try
            {
                var c = await _CustomerService.GetByIdAsync(id);
                await _CustomerService.DeleteAsync(id);

                return Ok(new { Message = "Customer information Deleted successfully", Customer = c.Name });

            }
            catch(Exception ex)
            {
                return BadRequest($"There's an Error {ex.Message}");
            }
        }

    }
}
