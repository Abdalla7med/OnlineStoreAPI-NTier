using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL;
using BLL;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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


        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetProductsAsync()
        {
            IEnumerable<Product> products = new List<Product>(); ;
            try
            {
                 products = await _productservice.GetAllAsync();
            } catch(Exception ex){
                return BadRequest(ex.Message);
            }

            return Ok(products);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            Product product;
            try
            {
                product = await _productservice.GetByIdAsync(id); // Handle exception from BLL
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if (product != null)
                return Ok(product);

            return NotFound("Product not found");  // Use NotFound instead of BadRequest for missing resource
        }

        [HttpGet("GetByName/{Name:alpha}")]
        public async Task<IActionResult> GetProductByName(string Name)
        {
            Product product;
            try
            {
                product = await _productservice.GetByIdAsync(Name); // Handle exception from BLL
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if (product != null)
                return Ok(product);

            return NotFound("Product not found");  // Use NotFound instead of BadRequest for missing resource
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            Product product;
            try
            {
                product = await _productservice.GetByIdAsync(id); // Handle exception from BLL
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if (product != null)
                return Ok(product);

            return NotFound("Product not found");  // Use NotFound instead of BadRequest for missing resource
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto productDto)
        {
            var products = await _productservice.GetAllAsync();
            var existProduct = products.FirstOrDefault(p => p.Name == productDto.Name);

            if (existProduct != null)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        Status = "Error",
                        Message = "Product already exists!"
                    }
                );
            }

            if (ModelState.IsValid)
            {
                int id = await _productservice.AddAsync(productDto);
                // Ensure the correct action and parameters are passed
                return CreatedAtAction(nameof(GetProductById), new { id = id }, productDto);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingProduct = await _productservice.GetByIdAsync(id);

                if (existingProduct == null)
                {
                    return NotFound(new { Status = "Error", Message = "Product not found!" });
                }

                await _productservice.UpdateAsync(id, productDto);

                return NoContent(); // 204 No Content on successful update
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = ex.Message });
            }
        }

    }
}
