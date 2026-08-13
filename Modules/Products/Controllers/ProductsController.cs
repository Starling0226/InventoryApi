using InventoryApi.Modules.Products.Dtos;
using InventoryApi.Modules.Products.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Modules.Products.Controllers
{
    [ApiController]
    [Route("products")]
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
            if (product == null) return NotFound(new { Message = "Producto no encontrado." });
            return Ok(product);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateProductDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newProduct = _productService.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = newProduct.Id }, newProduct); 
        }

        [HttpPatch("{id:int}")] 
        public IActionResult PartialUpdate(int id, [FromBody] PartialUpdateProductDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedProduct = _productService.PartialUpdate(id, dto);
            if (updatedProduct == null) return NotFound(new { Message = "Producto no encontrado." });

            return Ok(updatedProduct);
        }

        [HttpDelete("{id:int}")] 
        public IActionResult Delete(int id)
        {
            var isDeleted = _productService.Delete(id);
            if (!isDeleted) return NotFound(new { Message = "Producto no encontrado." });

            return Ok(new { Message = "Producto eliminado exitosamente." });
        }
    }
}