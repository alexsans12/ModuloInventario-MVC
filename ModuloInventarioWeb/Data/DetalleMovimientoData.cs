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
        var results = _db.SaveData("SPDetalleMovimiento_Actualizar", new { detalleMovimiento.Id });

        return results;
    }

    public Task Borrar(int id)
    {
        var results = _db.SaveData("SPDetalleMovimiento_Borrar", new { id });

        return results;
    }

    public Task Insertar(DetalleMovimiento detalleMovimiento)
    {
        var results = _db.SaveData("SPDetalleMovimiento_Guardar", new { detalleMovimiento.IdMovimiento });

        return results;
    }

    public async Task<DetalleMovimiento?> ObtenerPorId(int id)
    {
        var results = await _db.LoadData<DetalleMovimiento, dynamic>("SPDetalleMovimiento_Consultar", new { id });

        return results.FirstOrDefault();
    }

    public Task<IEnumerable<DetalleMovimiento>> ObtenerTodos()
    {
        return _db.LoadData<DetalleMovimiento, dynamic>("SPDetalleMovimiento_Consultar", new { });
    }
}

