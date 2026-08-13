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
            var products = _productService.GetAll();
            return Ok(products); // 200 OK
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            var product = _productService.GetById(id);
            if (product == null)
            {
                return NotFound(new { Message = "Producto no encontrado." }); // 404 Not Found
            }
            return Ok(product); // 200 OK
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateProductDto dto)
        {
            // ModelState verifica automáticamente los Data Annotations de nuestros DTOs
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400 Bad Request
            }

            var newProduct = _productService.Create(dto);
            // 201 Created y retorna la ruta para consultar el nuevo recurso
            return CreatedAtAction(nameof(GetById), new { id = newProduct.Id }, newProduct); 
        }

        [HttpPatch("{id:guid}")]
        public IActionResult PartialUpdate(Guid id, [FromBody] PartialUpdateProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400 Bad Request
            }

            var updatedProduct = _productService.PartialUpdate(id, dto);
            if (updatedProduct == null)
            {
                return NotFound(new { Message = "Producto no encontrado para actualizar." }); // 404 Not Found
            }

            return Ok(updatedProduct); // 200 OK
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var isDeleted = _productService.Delete(id);
            if (!isDeleted)
            {
                return NotFound(new { Message = "Producto no encontrado para eliminar." }); // 404 Not Found
            }

            return Ok(new { Message = "Producto eliminado exitosamente." }); // 200 OK
        }
    }
}