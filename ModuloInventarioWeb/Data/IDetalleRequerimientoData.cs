using System;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Data;

public interface IDetalleRequerimientoData
{
    Task<IEnumerable<DetalleRequerimiento>?> ObtenerPorRequerimiento(int id);
    Task Insertar(DetalleRequerimiento detalleRequerimiento);
    Task Borrar(int id);
}
