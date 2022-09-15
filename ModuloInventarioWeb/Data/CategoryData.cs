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
        return _db.LoadData<Category, dynamic>("SPCategory_GetAll", new { });
    }

    //public Task<IEnumerable<Category>> GetCategories() => _db.LoadData<Category, dynamic>("SPCategory_GetAll", new { });

    public async Task<Category?> GetCategory(int id)
    {
        var results = await _db.LoadData<Category, dynamic>("SPCategory_GetById", new { id });

        return results.FirstOrDefault();
    }
}

