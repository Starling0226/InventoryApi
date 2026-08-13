using InventoryApi.Modules.Categories.Dtos;
using InventoryApi.Modules.Categories.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Modules.Categories.Controllers
{
    [ApiController]
    [Route("categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_categoryService.GetAll());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var category = _categoryService.GetById(id);
            if (category == null) return NotFound(new { Message = "Categoría no encontrada." });
            return Ok(category);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateCategoryDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newCategory = _categoryService.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = newCategory.Id }, newCategory);
        }
    }
}