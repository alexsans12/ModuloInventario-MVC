using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Data
{
    public interface IRolData
    {
        Task<IEnumerable<Rol>> GetRol();
        Task<Rol?> GetRol(int ID_Rol);

        Task InsertRol(Rol rol);
        Task UpdateRol(Rol rol);
        Task DeleteRol(int ID_Rol);
    }
}
