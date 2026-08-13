using InventoryApi.Modules.Categories.Repositories;
using InventoryApi.Modules.Categories.Services;

namespace InventoryApi.Modules.Categories
{
    public static class CategoriesModule
    {
        public static IServiceCollection AddCategoriesModule(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<CategoryService>();
            return services;
        }
    }
}