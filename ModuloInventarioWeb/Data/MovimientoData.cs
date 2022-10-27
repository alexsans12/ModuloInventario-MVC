using System;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using ModuloInventarioWeb.DbAccess;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Data;

public class MovimientoData : IMovimientoData
{
    private readonly ISqlDataAccess _db;
    private readonly IConfiguration _configuration;

    public MovimientoData(ISqlDataAccess db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    public Task Actualizar(Movimiento movimiento)
    {
        var results = _db.SaveData("SPMovimiento_Actualizar", new { movimiento.Id, Fecha = movimiento.FechaCreacion, Tipo_Movimiento = movimiento.TipoMovimiento, movimiento.Descripcion, movimiento.Total, Id_Usuario = movimiento.IdUsuario });

        return results;
    }

    public Task Borrar(int id)
    {
        var results = _db.SaveData("SPMovimiento_Eliminar", new { id });

        return results;
    }

    public async Task<int> Insertar(Movimiento movimiento)
    {
        var mov = new DynamicParameters();
        mov.Add("@Id", 0, DbType.Int32, ParameterDirection.Output);
        mov.Add("@Fecha", movimiento.FechaCreacion);
        mov.Add("@TipoMovimiento", movimiento.TipoMovimiento);
        mov.Add("@Descripcion", movimiento.Descripcion);
        mov.Add("@Total", movimiento.Total);
        mov.Add("@IdUsuario", movimiento.IdUsuario);

        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnectionString"));

        var results = connection.Query<int>("SPMovimiento_Insertar", mov, commandType: CommandType.StoredProcedure);

        return mov.Get<int>("@Id");
    }

    public async Task<Movimiento?> ObtenerPorId(int Id)
    {
        var results = await _db.LoadData<Movimiento, dynamic>("SPMovimiento_Obtener", new { Id });

        return results.FirstOrDefault();
    }

    public Task<IEnumerable<Movimiento>> ObtenerTodos()
    {
        return _db.LoadData<Movimiento, dynamic>("SPMovimiento_Obtener", new { Id = 0 });
    }
}

