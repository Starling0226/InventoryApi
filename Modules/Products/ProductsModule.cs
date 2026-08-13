using InventoryApi.Modules.Products.Repositories;
using InventoryApi.Modules.Products.Services;

namespace InventoryApi.Modules.Products
{
    public static class ProductsModule
    {
        public static IServiceCollection AddProductsModule(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            
            services.AddScoped<ProductService>();
            
            return services;
        }
    }
}