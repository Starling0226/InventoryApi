using InventoryApi.Modules.Products.Dtos;
using InventoryApi.Modules.Products.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Modules.Products.Controllers
{
    [ApiController]
    [Route("products")]
    [Authorize] 
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_productService.GetAll());
        }

        [HttpGet("{id:int}")] 
        public IActionResult GetById(int id)
        {
            var product = _productService.GetById(id);
            return Ok(product);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateProductDto dto)
        {
            var newProduct = _productService.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = newProduct.Id }, newProduct); 
        }

        [HttpPatch("{id:int}")] 
        public IActionResult PartialUpdate(int id, [FromBody] PartialUpdateProductDto dto)
        {
            var updatedProduct = _productService.PartialUpdate(id, dto);
            return Ok(updatedProduct);
        }

        [HttpDelete("{id:int}")] 
        public IActionResult Delete(int id)
        {
            _productService.Delete(id);
            
            return NoContent(); 
        }
    }
}