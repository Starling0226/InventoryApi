using InventoryApi.Modules.Categories.Entities;

namespace InventoryApi.Modules.Categories.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();
        Category? GetById(int id);
        Category Create(Category category);
    }
}