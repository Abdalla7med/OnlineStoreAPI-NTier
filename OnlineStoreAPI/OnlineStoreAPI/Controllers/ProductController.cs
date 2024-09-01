using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL;
using BLL;
namespace OnlineStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private ProductService _productservice;
        public ProductController(ProductService service)
        {
            _productservice = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetProductsAsync()
        {
            var products = await _productservice.GetAllAsync();

            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productservice.GetByIdAsync(id);

            if (product != null)
                return Ok(product);

            return BadRequest("Product Deosn't exists");

        }
    }
}
