using System;
namespace ModuloInventarioWeb.Models;

public class DetalleMovimiento
{
    public int Id { get; set; }
    public int IdMovimiento { get; set; }
    public int IdProducto { get; set; } // Agregar la clase producto cuando ya este creada
    public int Cantidad { get; set; }
    public double PrecioUnidad { get; set; }
    public double Subtotal { get; set; }
}

