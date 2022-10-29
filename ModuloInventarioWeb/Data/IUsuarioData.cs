using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Data
{
    public interface IUsuarioData
    {
        Task<IEnumerable<Usuario>> GetUsuario();
        Task<Usuario?> GetUsuario(int Id);
        Task<Usuario?> ObtenerPorNombre(String nombre);
        Task InsertUsuario(Usuario usuario);
        Task UpdateUsuario(Usuario usuario);
        Task DeleteUsuario(int ID_Usuario);
    }
}
