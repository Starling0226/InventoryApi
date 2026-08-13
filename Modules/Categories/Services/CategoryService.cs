using InventoryApi.Modules.Categories.Dtos;
using InventoryApi.Modules.Categories.Entities;
using InventoryApi.Modules.Categories.Repositories;

namespace InventoryApi.Modules.Categories.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<CategoryResponseDto> GetAll()
        {
            var categories = _repository.GetAll();
            return categories.Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
        }

        public CategoryResponseDto? GetById(int id)
        {
            var category = _repository.GetById(id);
            if (category == null) return null;

            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public CategoryResponseDto Create(CreateCategoryDto dto)
        {
            var newCategory = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            var createdCategory = _repository.Create(newCategory);

            return new CategoryResponseDto
            {
                Id = createdCategory.Id,
                Name = createdCategory.Name,
                Description = createdCategory.Description
            };
        }
    }
}