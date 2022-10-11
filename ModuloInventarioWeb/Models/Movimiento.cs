﻿using System;
namespace ModuloInventarioWeb.Models;

public class Movimiento
{
    public int Id { get; set; }
    public DateTime FechaCreacion { get; set; }
    public bool TipoMovimiento { get; set; }
    public String Descripcion { get; set; }
    public double Total { get; set; }
    public int IdUsuario { get; set; } // Agregar la clase usuario cuando ya este creada
}
