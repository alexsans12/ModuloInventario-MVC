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

    public async Task<Usuario?> GetUsuario(int Id)
    {
        var results = await _db.LoadData<Usuario, dynamic>("SPUsuario_Obtener", new { Id });

        return results.FirstOrDefault();
    }

    public Task InsertUsuario(Usuario usuario)
    {
        var results = _db.SaveData("SPCategory_Insert", new { Id = usuario.Id_Usuario, usuario.Nombre, usuario.Contrasena, usuario.Foto_Perfil });
        return results;
    }

    public Task UpdateUsuario(Usuario usuario)
    {
        var results = _db.SaveData("SPUsuario_Update", new { Id = usuario.Id_Usuario, usuario.Nombre, usuario.Contrasena, usuario.Foto_Perfil });
        return results;
    }

    public Task DeleteUsuario(Usuario usuario)
    {
        var results = _db.SaveData("SPCategory_Delete", new { usuario.Id_Usuario });
        return results;
    }

    public async Task<Usuario?> ObtenerPorNombre(String nombre)
    {
        var results = await _db.LoadData<Usuario, dynamic>("SPUsuario_ObtenerPorNombre", new { nombre });

        return results.FirstOrDefault();
    }
}
