using System;
using ModuloInventarioWeb.DbAccess;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Data;

public class MovimientoData : IMovimientoData
{
    private readonly ISqlDataAccess _db;

    public MovimientoData(ISqlDataAccess db)
    {
        _db = db;
    }

    public Task Actualizar(Movimiento movimiento)
    {
        var results = _db.SaveData("SPMovimiento_Actualizar", new { movimiento.Id });

        return results;
    }

    public Task Borrar(int id)
    {
        var results = _db.SaveData("SPMovimiento_Borrar", new { id });

        return results;
    }

    public Task Insertar(Movimiento movimiento)
    {
        var results = _db.SaveData("SPMovimiento_Guardar", new { movimiento.IdUsuario });

        return results;
    }

    public async Task<Movimiento?> ObtenerPorId(int Id)
    {
        var results = await _db.LoadData<Movimiento, dynamic>("SPMovimiento_Obtener", new { Id });

        return results.FirstOrDefault();
    }

    public Task<IEnumerable<Movimiento>> ObtenerTodos()
    {
        return _db.LoadData < Movimiento, dynamic>("SPMovimiento_Obtener", new { Id = 0 });
    }
}

