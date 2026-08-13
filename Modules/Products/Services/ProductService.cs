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

        public IEnumerable<Product> GetAll()
        {
            return _repository.GetAll();
        }

        public Product? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public Product Create(CreateProductDto dto)
        {
            var newProduct = new Product
            {
                Uuid = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock
            };

            return _repository.Create(newProduct);
        }

        public Product? PartialUpdate(int id, PartialUpdateProductDto dto)
        {
            var product = _repository.GetById(id);
            if (product == null) return null;

            if (dto.Name is not null) product.Name = dto.Name;
            if (dto.Description is not null) product.Description = dto.Description;
            if (dto.Price.HasValue) product.Price = dto.Price.Value;
            if (dto.Stock.HasValue) product.Stock = dto.Stock.Value;

            return _repository.Update(product);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }
    }
}