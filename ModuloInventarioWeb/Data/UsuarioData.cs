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
        return _db.LoadData<Usuario, dynamic>("SPCategory_GetAll", new { });
    }

    public async Task<Usuario?> GetUsuario(int ID_Usuario)
    {
        var results = await _db.LoadData<Usuario, dynamic>("SPCategory_GetById", new { ID_Usuario });

        return results.FirstOrDefault();
    }

    public Task InsertUsuario(Usuario usuario)
    {
        var results = _db.SaveData("SPCategory_Insert", new { usuario.ID_Usuario, usuario.Nombre, usuario.Contrasena, usuario.Foto_Perfil });
        return results;
    }

    public Task UpdateUsuario(Usuario usuario)
    {
        var results = _db.SaveData("SPCategory_Update", new { usuario.ID_Usuario, usuario.Nombre, usuario.Contrasena, usuario.Foto_Perfil });
        return results;
    }

    public Task DeleteUsuario(Usuario usuario)
    {
        var results = _db.SaveData("SPCategory_Delete", new { usuario.ID_Usuario });
        return results;
    }

   
}
