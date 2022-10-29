using System;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Data;

public interface IKardexData
{
    Task<IEnumerable<Kardex>> GetAll();
    Task<Kardex?> GetByIdMovimiento(int id);
    Task<Kardex?> GetByIdRequerimiento(int id);
    Task<IEnumerable<Kardex>> GetByTipo(bool tipo);
    Task<IEnumerable<Kardex>> GetByProducto(int id);
    Task InsertKardex(Kardex kardex);
    Task DeleteKardexMov(int id);
    Task DeleteKardexReq(int id);
}

