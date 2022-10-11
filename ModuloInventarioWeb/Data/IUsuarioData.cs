using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Data
{
    public interface IUsuarioData
    {
        Task<IEnumerable<Usuario>> GetUsuario();
        Task<Usuario?> GetUsuario(int ID_Usuario);

        Task InsertUsuario(Usuario usuario);
        Task UpdateUsuario(Usuario usuario);
        Task DeleteUsuario(Usuario ID_Usuario);
    }
}
