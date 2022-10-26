using System;
using ModuloInventarioWeb.DbAccess;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Data;

public class RolData : IRolData
{
    private readonly ISqlDataAccess _db;

    public RolData(ISqlDataAccess db)
    {
        _db = db;
    }

    public Task<IEnumerable<Rol>> GetRol()
    {
        return _db.LoadData<Rol, dynamic>("SPRol_GetAll", new { });
    }

    public async Task<Rol?> GetRol(int ID_Rol)
    {
        var results = await _db.LoadData<Rol, dynamic>("SPCategory_GetById", new { ID_Rol });

        return results.FirstOrDefault();
    }

    public Task InsertRol(Rol rol)
    {
        var results = _db.SaveData("SPRol_Insert", new { rol.ID_Rol, rol.Nombre, rol.Descripcion, rol.Ingreso_Requerimiento, rol.Autorizar_Requerimineto, rol.Borrar_Requerimiento, rol.Ingreso_Movimiento });
        return results;
    }

    public Task UpdateRol(Rol rol)
    {
        var results = _db.SaveData("SPRol_Update", new { rol.ID_Rol, rol.Nombre, rol.Descripcion, rol.Ingreso_Requerimiento, rol.Autorizar_Requerimineto, rol.Borrar_Requerimiento, rol.Ingreso_Movimiento });
        return results;
    }

    public Task DeleteRol(int ID_Rol)
    {
        var results = _db.SaveData("SPRol_Delete", new { ID_Rol });
        return results;
    }
}
