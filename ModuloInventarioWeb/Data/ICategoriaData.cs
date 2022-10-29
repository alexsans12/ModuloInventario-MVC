using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Data
{
    public interface ICategoriaData
    {
        Task<IEnumerable<Categoria>> GetCategoria();
        Task<Categoria?> GetCategoria(int ID_Categoria);
        Task InsertCategoria(Categoria categoria);
        Task UpdateCategoria(Categoria categoria);
        Task DeleteCategoria(int ID_Categoria);
    }
}
