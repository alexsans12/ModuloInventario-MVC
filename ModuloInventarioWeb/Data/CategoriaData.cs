using System;
using ModuloInventarioWeb.DbAccess;
using ModuloInventarioWeb.Models;


namespace ModuloInventarioWeb.Data;

public class CategoriaData : ICategoriaData
{
    private readonly ISqlDataAccess _db;

    public CategoriaData(ISqlDataAccess db)
    {
        _db = db;
    }


    public Task<IEnumerable<Categoria>> GetCategoria()
    {
        return _db.LoadData<Categoria, dynamic>("SPCategoria_GetAll", new { });
    }

    public async Task<Categoria?> GetCategoria(int ID_Categoria)
    {
        var results = await _db.LoadData<Categoria, dynamic>("SPCategoria_GetById", new { ID_Categoria });

        return results.FirstOrDefault();
    }

    public Task InsertCategoria(Categoria categoria)
    {
        var results = _db.SaveData("SPCategoria_Insertar", new {   categoria.Nombre, categoria.Descripcion });
        return results;
    }

    public Task UpdateCategoria(Categoria categoria)
    {
        var results = _db.SaveData("SPCategoria_Actualizar", new { categoria.ID_Categoria, categoria.Nombre, categoria.Descripcion });
        return results;
    }

    public Task DeleteCategoria(int ID_Categoria)
    {
        var results = _db.SaveData("SPCategoria_Eliminar", new { ID_Categoria });
        return results;
    }

   
}
