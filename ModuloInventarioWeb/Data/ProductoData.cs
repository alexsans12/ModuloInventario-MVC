using System;
using ModuloInventarioWeb.DbAccess;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Data;

public class ProductoData : IProductoData
{
    private readonly ISqlDataAccess _db;

    public ProductoData(ISqlDataAccess db)
    {
        _db = db;
    }

    public Task<IEnumerable<Producto>> GetProducto()
    {
        return _db.LoadData<Producto, dynamic>("SPProducto_Obtener", new { });
    }

    public async Task<Producto?> GetProducto(int ID_Producto)
    {
        var results = await _db.LoadData<Producto, dynamic>("SPProducto_Obtener", new { ID_Producto });

        return results.FirstOrDefault();
    }

    public Task InsertProducto(Producto producto)
    {
        var results = _db.SaveData("SPCategory_Insert", new { producto.Codigo, producto.Nombre, producto.Descripcion, producto.Stock, producto.Precio, });
        return results;
    }

    public Task UpdateProducto(Producto producto)
    {
        var results = _db.SaveData("SPCategory_Update", new { producto.ID_Producto, producto.Codigo, producto.Descripcion,producto.ID_Categoria, producto.Stock, producto.Precio, producto.Imagen_Producto });
        return results;
    }

    public Task DeleteProducto(int ID_Producto)
    {
        var results = _db.SaveData("SPCategory_Delete", new { ID_Producto });
        return results;
    }


}
