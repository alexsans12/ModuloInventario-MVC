using System;
using ModuloInventarioWeb.DbAccess;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Data;

public class KardexData : IKardexData
{
    private readonly ISqlDataAccess _db;

    public KardexData(ISqlDataAccess db)
    {
        _db = db;
    }

    public Task DeleteKardexMov(int id)
    {
        var results = _db.SaveData("SPKardex_EliminarMov", new { IdMovimiento = id });
        return results;
    }

    public Task DeleteKardexReq(int id)
    {
        var results = _db.SaveData("SPKardex_EliminarReq", new { IdRequerimiento = id });
        return results;
    }

    public Task<IEnumerable<Kardex>> GetAll()
    {
        return _db.LoadData<Kardex, dynamic>("SPKardex_GetAll", new { });
    }

    public async Task<Kardex?> GetByIdMovimiento(int id)
    {
        var results = await _db.LoadData<Kardex, dynamic>("SPKardex_GetByIdMovimiento", new { IdMovimiento = id });

        return results.FirstOrDefault();
    }

    public async Task<Kardex?> GetByIdRequerimiento(int id)
    {
        var results = await _db.LoadData<Kardex, dynamic>("SPKardex_GetByIdRequerimiento", new { IdRequerimiento = id });

        return results.FirstOrDefault();
    }

    public Task<IEnumerable<Kardex>> GetByTipo(bool tipo)
    {
        var results = _db.LoadData<Kardex, dynamic>("SPCategoria_GetById", new { TipoMovimiento = tipo });
        return results;
    }

    public Task<IEnumerable<Kardex>> GetByProducto(int id)
    {
        var results = _db.LoadData<Kardex, dynamic>("SPKardex_GetByProducto", new { IdProducto = id });
        return results;
    }

    public Task InsertKardex(Kardex kardex)
    {
        var results = _db.SaveData("SPKardex_Insertar", new { kardex.FechaCreacion, kardex.Motivo, kardex.Cantidad, kardex.IdUsuario, kardex.IdProducto, kardex.IdMovimiento, kardex.IdRequerimiento, kardex.TipoMovimiento, kardex.Total, Costo = kardex.PrecioUnidad, kardex.StockAnterior, kardex.StockActual });
        return results;
    }
}
