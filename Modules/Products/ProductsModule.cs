using InventoryApi.Modules.Products.Services;

namespace InventoryApi.Modules.Products
{
    public static class ProductsModule
    {
        public static IServiceCollection AddProductsModule(this IServiceCollection services)
        {
            // Registramos el servicio para que el Controller pueda inyectarlo
            services.AddScoped<ProductService>();
            return services;
        }
    }
}