using System;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Data;

public interface IDetalleMovimientoData
{
    Task<IEnumerable<DetalleMovimiento>> ObtenerTodos();
    Task<DetalleMovimiento?> ObtenerPorId(int id);
    Task Insertar(DetalleMovimiento detalleMovimiento);
    Task Actualizar(DetalleMovimiento detalleMovimiento);
    Task Borrar(int id);
}

