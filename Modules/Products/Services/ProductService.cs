using InventoryApi.Modules.Products.Dtos;
using InventoryApi.Modules.Products.Entities;

namespace InventoryApi.Modules.Products.Services
{
    public class ProductService
    {
        private static readonly List<Product> _products = new();

        public List<Product> GetAll()
        {
            return _products;
        }

        public Product? GetById(Guid id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public Product Create(CreateProductDto dto)
        {
            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock
            };

            _products.Add(newProduct);
            return newProduct;
        }

        public Product? PartialUpdate(Guid id, PartialUpdateProductDto dto)
        {
            var product = GetById(id);
            if (product == null) return null;

            if (dto.Name is not null) product.Name = dto.Name;
            if (dto.Description is not null) product.Description = dto.Description;
            if (dto.Price.HasValue) product.Price = dto.Price.Value;
            if (dto.Stock.HasValue) product.Stock = dto.Stock.Value;

            return product;
        }

        public bool Delete(Guid id)
        {
            var product = GetById(id);
            if (product == null) return false;

            _products.Remove(product);
            return true;
        }
    }
}