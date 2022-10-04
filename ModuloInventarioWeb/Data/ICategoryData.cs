using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Data
{
    public interface ICategoryData
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<Category?> GetCategory(int id);

        Task InsertCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(int id);
    }
}