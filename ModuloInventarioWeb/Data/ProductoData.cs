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
        return _db.LoadData<Producto, dynamic>("SPProducto_GetAll", new { });
    }

    public async Task<Producto?> GetProducto(int ID_Producto)
    {
        var results = await _db.LoadData<Producto, dynamic>("SPProducto_GetById", new { ID_Producto });

        return results.FirstOrDefault();
    }

    public Task InsertProducto(Producto producto)
    {
        var results = _db.SaveData("SPProducto_Insertar", new { producto.Codigo, producto.Nombre, producto.Descripcion, producto.Stock, producto.Precio, producto.Imagen_Producto, ID_Categoria = producto.ID_Categoria });
        return results;
    }

    public Task UpdateProducto(Producto producto)
    {
        var results = _db.SaveData("SPProducto_Actualizar", new { producto.ID_Producto, producto.Codigo, producto.Nombre, producto.Descripcion, producto.Stock, producto.Precio, producto.Imagen_Producto, ID_Categoria = producto.ID_Categoria });
        return results;
    }

    public Task DeleteProducto(int ID_Producto)
    {
        var results = _db.SaveData("SPProducto_Eliminar", new { ID_Producto });
        return results;
    }


}
