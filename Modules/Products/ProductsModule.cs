using System.ComponentModel.DataAnnotations;
using InventoryApi.Modules.Products.Dtos;
using InventoryApi.Modules.Products.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Modules.Products
{
    public static class ProductsModule
    {
        private static readonly List<Product> _products = new()
        {
            new Product { Id = Guid.NewGuid(), Name = "Laptop Lenovo LOQ", Description = "Laptop Gamer", Price = 950.50m, Stock = 10 },
            new Product { Id = Guid.NewGuid(), Name = "Monitor Dell 24", Description = "Monitor Externo FHD", Price = 150.00m, Stock = 5 }
        };

        // Método de extensión para mapear los endpoints en Program.cs
        public static void MapProductEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/products");

            group.MapGet("/", ([FromQuery] string? name) =>
            {
                if (!string.IsNullOrEmpty(name))
                {
                    var filtered = _products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
                    return filtered.Any() ? Results.Ok(filtered) : Results.NotFound(new { Message = "No se encontraron productos con ese nombre." });
                }
                return Results.Ok(_products);
            });

            group.MapGet("/{id:guid}", (Guid id) =>
            {
                var product = _products.FirstOrDefault(p => p.Id == id);
                return product is not null ? Results.Ok(product) : Results.NotFound(new { Message = "Producto no encontrado." });
            });

            group.MapPost("/", (CreateProductDto dto) =>
            {
                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(dto);
                if (!Validator.TryValidateObject(dto, context, validationResults, true))
                {
                    return Results.BadRequest(validationResults.Select(r => r.ErrorMessage));
                }

                var newProduct = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    Stock = dto.Stock
                };

                _products.Add(newProduct);
                return Results.Created($"/products/{newProduct.Id}", newProduct);
            });

            group.MapPut("/{id:guid}", (Guid id, UpdateProductDto dto) =>
            {
                var product = _products.FirstOrDefault(p => p.Id == id);
                if (product is null) return Results.NotFound(new { Message = "Producto no encontrado para actualizar." });

                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(dto);
                if (!Validator.TryValidateObject(dto, context, validationResults, true))
                {
                    return Results.BadRequest(validationResults.Select(r => r.ErrorMessage));
                }

                product.Name = dto.Name;
                product.Description = dto.Description;
                product.Price = dto.Price;
                product.Stock = dto.Stock;

                return Results.Ok(product);
            });

            group.MapPatch("/{id:guid}", (Guid id, PartialUpdateProductDto dto) =>
            {
                var product = _products.FirstOrDefault(p => p.Id == id);
                if (product is null) return Results.NotFound(new { Message = "Producto no encontrado para modificar." });

                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(dto);
                if (!Validator.TryValidateObject(dto, context, validationResults, true))
                {
                    return Results.BadRequest(validationResults.Select(r => r.ErrorMessage));
                }

                if (dto.Name is not null) product.Name = dto.Name;
                if (dto.Description is not null) product.Description = dto.Description;
                if (dto.Price.HasValue) product.Price = dto.Price.Value;
                if (dto.Stock.HasValue) product.Stock = dto.Stock.Value;

                return Results.Ok(product);
            });

            group.MapDelete("/{id:guid}", (Guid id) =>
            {
                var product = _products.FirstOrDefault(p => p.Id == id);
                if (product is null) return Results.NotFound(new { Message = "Producto no encontrado para eliminar." });

                _products.Remove(product);
                return Results.Ok(new { Message = "Producto eliminado exitosamente." });
            });
        }
    }
}