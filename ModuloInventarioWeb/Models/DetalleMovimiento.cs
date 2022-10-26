using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModuloInventarioWeb.Models;

public class DetalleMovimiento
{
    [Required]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Movimiento")]
    public int IdMovimiento { get; set; }

    [Required]
    [ForeignKey("Producto")]
    public int IdProducto { get; set; } // Agregar la clase producto cuando ya este creada

    [Required]
    public int Cantidad { get; set; }

    [Required]
    public double PrecioUnidad { get; set; }

    [Required]
    public double Subtotal { get; set; }

    public virtual Producto Producto { get; set; }
}

