using System;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Data;

public interface IKardex
{
    Task<IEnumerable<Kardex>> GetAll();
    Task<Kardex?> GetById(int id);
    Task<IEnumerable<Kardex>> GetByTipo(bool tipo);
    Task InsertKardex(Kardex kardex);
    Task DeleteKardex(int id);
}

