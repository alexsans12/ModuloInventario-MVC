using System;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Data;

public interface IMovimientoData
{
    Task<IEnumerable<Movimiento>> ObtenerTodos();
    Task<Movimiento?> ObtenerPorId(int id);
    Task Insertar(Movimiento movimiento);
    Task Actualizar(Movimiento movimiento);
    Task Borrar(int id);
}

