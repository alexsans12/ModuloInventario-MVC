using System;
using ModuloInventarioWeb.DbAccess;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Data;

public class CategoryData : ICategoryData
{
    private readonly ISqlDataAccess _db;

    public CategoryData(ISqlDataAccess db)
    {
        _db = db;
    }


    public Task<IEnumerable<Category>> GetCategories()
    {
        return _db.LoadData<Category, dynamic>("SPCategoria_GetAll", new { });
    }

    public async Task<Category?> GetCategory(int id)
    {
        var results = await _db.LoadData<Category, dynamic>("SPCategoria_GetById", new { id });

        return results.FirstOrDefault();
    }

    public Task InsertCategory(Category category)
    {
        var results = _db.SaveData("SPCategory_Insert", new { category.Name, category.DisplayOrder });
        return results;
    }

    public Task UpdateCategory(Category category)
    {
        var results = _db.SaveData("SPCategory_Update", new { category.Id, category.Name, category.DisplayOrder });
        return results;
    }

    public Task DeleteCategory(int id)
    {
        var results = _db.SaveData("SPCategory_Delete", new { id });
        return results;
    }
}

