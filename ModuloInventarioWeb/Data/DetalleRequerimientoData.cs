using System;
using ModuloInventarioWeb.DbAccess;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Data;

public class DetalleRequerimientoData : IDetalleRequerimientoData
{
    private readonly ISqlDataAccess _db;

    public DetalleRequerimientoData(ISqlDataAccess db)
    {
        _db = db;
    }

    public Task Borrar(int id)
    {
        var results = _db.SaveData("SPDetalleRequerimiento_Eliminar", new { id });

        return results;
    }

    public Task Insertar(DetalleRequerimiento detalleRequerimiento)
    {
        var results = _db.SaveData("SPDetalleRequerimiento_Insertar", new { detalleRequerimiento.IdRequerimiento, detalleRequerimiento.IdProducto, detalleRequerimiento.Cantidad, detalleRequerimiento.PrecioUnidad, detalleRequerimiento.Subtotal });

        return results;
    }

    public Task<IEnumerable<DetalleRequerimiento>?> ObtenerPorRequerimiento(int id)
    {
        return _db.LoadData<DetalleRequerimiento, dynamic>("SPDetalleRequerimiento_ObtenerPorRequerimiento", new { id });
    }
}

