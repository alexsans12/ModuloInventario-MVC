using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Data
{
    public interface IProductoData
    {
        Task<IEnumerable<Producto>> GetProducto();
        Task<Producto?> GetProducto(int ID_Producto);

        Task InsertProducto(Producto producto);
        Task UpdateProducto(Producto producto);
        Task DeleteProducto(int ID_Producto);
    }
}
