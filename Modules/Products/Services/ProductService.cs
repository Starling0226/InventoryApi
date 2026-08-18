using InventoryApi.Modules.Products.Dtos;
using InventoryApi.Modules.Products.Entities;
using InventoryApi.Modules.Products.Repositories;
using InventoryApi.Exceptions; 

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

        public ProductResponseDto GetById(int id)
        {
            var product = _repository.GetById(id);
            
            if (product == null) 
            {
                throw new NotFoundException($"El producto con el ID {id} no fue encontrado.");
            }
            
            return MapToResponseDto(product);
        }

        public ProductResponseDto Create(CreateProductDto dto)
        {
            // Validaciones de negocio (Lanzan error 400)
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new BadRequestException("El nombre del producto es obligatorio.");
            }
            if (dto.Price <= 0)
            {
                throw new BadRequestException("El precio del producto debe ser mayor a cero.");
            }
            if (dto.Stock < 0)
            {
                throw new BadRequestException("El stock no puede ser negativo.");
            }

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

        public ProductResponseDto PartialUpdate(int id, PartialUpdateProductDto dto)
        {
            var product = _repository.GetById(id);
            
            if (product == null) 
            {
                throw new NotFoundException($"El producto con el ID {id} no fue encontrado.");
            }

            if (dto.Name is not null && string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new BadRequestException("El nombre del producto no puede quedar vacío.");
            }
            if (dto.Price.HasValue && dto.Price.Value <= 0)
            {
                throw new BadRequestException("El precio del producto debe ser mayor a cero.");
            }
            if (dto.Stock.HasValue && dto.Stock.Value < 0)
            {
                throw new BadRequestException("El stock no puede ser negativo.");
            }

            if (dto.Name is not null) product.Name = dto.Name;
            if (dto.Description is not null) product.Description = dto.Description;
            if (dto.Price.HasValue) product.Price = dto.Price.Value;
            if (dto.Stock.HasValue) product.Stock = dto.Stock.Value;
            if (dto.CategoryId.HasValue) product.CategoryId = dto.CategoryId.Value;

            var updatedProduct = _repository.Update(product);
            
            var productWithCategory = _repository.GetById(updatedProduct.Id);
            return MapToResponseDto(productWithCategory!);
        }

        public void Delete(int id)
        {
            var product = _repository.GetById(id);
            if (product == null) 
            {
                throw new NotFoundException($"El producto con el ID {id} no fue encontrado.");
            }

            _repository.Delete(id);
        }

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