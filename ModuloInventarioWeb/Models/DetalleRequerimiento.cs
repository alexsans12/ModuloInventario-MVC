using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModuloInventarioWeb.Models;

public class DetalleRequerimiento
{
    [Required]
    [ForeignKey("Requerimiento")]
    public int IdRequerimiento { get; set; }

    [Required]
    [ForeignKey("Producto")]
    public int IdProducto { get; set; }

    [Required]
    public int Cantidad { get; set; }

    [Required]
    public double PrecioUnidad { get; set; }

    [Required]
    public double Subtotal { get; set; }

    public virtual Producto? Producto { get; set; }
}
