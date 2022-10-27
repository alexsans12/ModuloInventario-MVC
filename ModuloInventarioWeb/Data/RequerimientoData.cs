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

    public Task Actualizar(Requerimiento requerimiento)
    {
        var results = _db.SaveData("SP_Actualizar", new { requerimiento.Id });

        return results;
    }

    public Task Borrar(int id)
    {
        var results = _db.SaveData("SP_Eliminar", new { id });

        return results;
    }

    public async Task<int> Insertar(Requerimiento requerimiento)
    {
        var mov = new DynamicParameters();
        mov.Add("@Id", 0, DbType.Int32, ParameterDirection.Output);
        mov.Add("@Fecha", requerimiento.FechaIngreso);
        mov.Add("@IdUsuario", requerimiento.IdUsuarioIngreso);

        using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnectionString"));

        var results = connection.Query<int>("SP_Insertar", mov, commandType: CommandType.StoredProcedure);

        return mov.Get<int>("@Id");
    }

    public async Task<Requerimiento?> ObtenerPorId(int Id)
    {
        var results = await _db.LoadData<Requerimiento, dynamic>("SPMovimiento_Obtener", new { Id });

        return results.FirstOrDefault();
    }

    public Task<IEnumerable<Requerimiento>> ObtenerTodos()
    {
        return _db.LoadData<Requerimiento, dynamic>("SPMovimiento_Obtener", new { Id = 0 });
    }
}