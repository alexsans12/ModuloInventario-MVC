using System;
using ModuloInventarioWeb.DbAccess;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Data;

public class DetalleMovimientoData : IDetalleMovimientoData
{
    private readonly ISqlDataAccess _db;

    public DetalleMovimientoData(ISqlDataAccess db)
    {
        _db = db;
    }

    public Task Actualizar(DetalleMovimiento detalleMovimiento)
    {
        var results = _db.SaveData("SPDetalleMovimiento_Actualizar", new { Id_Producto = detalleMovimiento.IdProducto, detalleMovimiento.Cantidad, Precio_Unidad = detalleMovimiento.PrecioUnidad, detalleMovimiento.Subtotal });

        return results;
    }

    public Task Borrar(int idMovimiento)
    {
        var results = _db.SaveData("SPDetalleMovimiento_Eliminar", new { idMovimiento });

        return results;
    }

    public Task Insertar(DetalleMovimiento detalleMovimiento)
    {
        var results = _db.SaveData("SPDetalleMovimiento_Insertar", new { detalleMovimiento.IdMovimiento, detalleMovimiento.IdProducto, detalleMovimiento.Cantidad, detalleMovimiento.PrecioUnidad, detalleMovimiento.Subtotal });

        return results;
    }

    public Task<IEnumerable<DetalleMovimiento>?> ObtenerPorMovimiento(int id)
    {
        return _db.LoadData<DetalleMovimiento, dynamic>("SPDetalleMovimiento_ObtenerPorMovimiento", new { id });
    }

    public Task<IEnumerable<DetalleMovimiento>> ObtenerTodos()
    {
        return _db.LoadData<DetalleMovimiento, dynamic>("SPDetalleMovimiento_Consultar", new { });
    }
}

