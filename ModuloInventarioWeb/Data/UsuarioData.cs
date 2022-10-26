using System;
using ModuloInventarioWeb.DbAccess;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Data;

public class UsuarioData : IUsuarioData
{
    private readonly ISqlDataAccess _db;

    public UsuarioData(ISqlDataAccess db)
    {
        _db = db;
    }

    public Task<IEnumerable<Usuario>> GetUsuario()
    {
        return _db.LoadData<Usuario, dynamic>("SPUsuario_GetAll", new { });
    }

    public async Task<Usuario?> GetUsuario(int ID_Usuario)
    {
        var results = await _db.LoadData<Usuario, dynamic>("SPUsuario_GetById", new { ID_Usuario });

        return results.FirstOrDefault();
    }

    public Task InsertUsuario(Usuario usuario)
    {
        var results = _db.SaveData("SPUsuario_Insertar", new { usuario.Nombre, usuario.Contrasena, usuario.Foto_Perfil });
        return results;
    }

    public Task UpdateUsuario(Usuario usuario)
    {
        var results = _db.SaveData("SPUsuario_Actualizar", new { usuario.ID_Usuario, usuario.Nombre, usuario.Contrasena, usuario.Foto_Perfil });
        return results;
    }

    public Task DeleteUsuario(int ID_Usuario)
    {
        var results = _db.SaveData("SPUsuario_Eliminar", new { ID_Usuario });
        return results;
    }

   
}
