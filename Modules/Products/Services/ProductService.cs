using InventoryApi.Modules.Products.Dtos;
using InventoryApi.Modules.Products.Entities;
using InventoryApi.Modules.Products.Repositories;

namespace InventoryApi.Modules.Products.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<ProductResponseDto> GetAll()
        {
            var products = _repository.GetAll();
            return products.Select(MapToResponseDto);
        }

        public ProductResponseDto? GetById(int id)
        {
            var product = _repository.GetById(id);
            return product == null ? null : MapToResponseDto(product);
        }

        public ProductResponseDto Create(CreateProductDto dto)
        {
            var newProduct = new Product
            {
                Uuid = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId 
            };

            var createdProduct = _repository.Create(newProduct);
            
            var productWithCategory = _repository.GetById(createdProduct.Id);
            return MapToResponseDto(productWithCategory!);
        }

        public ProductResponseDto? PartialUpdate(int id, PartialUpdateProductDto dto)
        {
            var product = _repository.GetById(id);
            if (product == null) return null;

            if (dto.Name is not null) product.Name = dto.Name;
            if (dto.Description is not null) product.Description = dto.Description;
            if (dto.Price.HasValue) product.Price = dto.Price.Value;
            if (dto.Stock.HasValue) product.Stock = dto.Stock.Value;
            if (dto.CategoryId.HasValue) product.CategoryId = dto.CategoryId.Value;

            var updatedProduct = _repository.Update(product);
            
            var productWithCategory = _repository.GetById(updatedProduct.Id);
            return MapToResponseDto(productWithCategory!);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }

        // Método auxiliar privado para no repetir código de mapeo
        private ProductResponseDto MapToResponseDto(Product product)
        {
            return new ProductResponseDto
            {
                Id = product.Id,
                Uuid = product.Uuid,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? "Sin categoría"
            };
        }
    }
}