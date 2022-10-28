using System;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using ModuloInventarioWeb.DbAccess;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Data;

public class RequerimientoData : IRequerimientoData
{
    private readonly ISqlDataAccess _db;
    private readonly IConfiguration _configuration;

    public RequerimientoData(ISqlDataAccess db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    public Task Autorizar(Requerimiento requerimiento)
    {
        var results = _db.SaveData("SPRequerimiento_Autorizar", new { requerimiento.Id, IdUsuario = requerimiento.IdUsuarioAutorizo, Fecha = requerimiento.FechaAutorizo, requerimiento.Estado });

        return results;
    }

    public Task Actualizar(Requerimiento requerimiento)
    {
        var results = _db.SaveData("SPRequerimiento_Actualizar", new { requerimiento.Id, IdUsuario = requerimiento.IdUsuarioIngreso, Fecha = requerimiento.FechaCreacion, requerimiento.Motivo, requerimiento.Total, requerimiento.Estado });

        return results;
    }

    public Task Borrar(int id)
    {
        var results = _db.SaveData("SPRequerimiento_Eliminar", new { id });

        return results;
    }

    public async Task<int> Insertar(Requerimiento requerimiento)
    {
        var mov = new DynamicParameters();
        mov.Add("@Id", 0, DbType.Int32, ParameterDirection.Output);
        mov.Add("@IdUsuario", requerimiento.IdUsuarioIngreso);
        mov.Add("@Fecha", requerimiento.FechaCreacion);
        mov.Add("@Motivo", requerimiento.Motivo);
        mov.Add("@Total", requerimiento.Total);
        mov.Add("@Estado", requerimiento.Estado);

        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnectionString"));

        var results = connection.Query<int>("SPRequerimiento_Insertar", mov, commandType: CommandType.StoredProcedure);

        return mov.Get<int>("@Id");
    }

    public async Task<Requerimiento?> ObtenerPorId(int Id)
    {
        var results = await _db.LoadData<Requerimiento, dynamic>("SPRequerimiento_Obtener", new { Id });

        return results.FirstOrDefault();
    }

    public Task<IEnumerable<Requerimiento>> ObtenerTodos()
    {
        return _db.LoadData<Requerimiento, dynamic>("SPRequerimiento_Obtener", new { Id = 0 });
    }
}