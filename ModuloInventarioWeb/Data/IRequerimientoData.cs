using System;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Data;

public interface IRequerimientoData
{
    Task<IEnumerable<Requerimiento>> ObtenerTodos();
    Task<Requerimiento?> ObtenerPorId(int id);
    Task<int> Insertar(Requerimiento requerimiento);
    Task Actualizar(Requerimiento requerimiento);
    Task Autorizar(Requerimiento requerimiento);
    Task Borrar(int id);
}
