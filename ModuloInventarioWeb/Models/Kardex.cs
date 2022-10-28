using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModuloInventarioWeb.Models;

public class Kardex
{
    [Key]
    public int Id { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    [Required]
    public string Motivo { get; set; }

    [Required]
    public int Cantidad { get; set; }

    [Required]
    [ForeignKey("Usuario")]
    public int IdUsuario { get; set; }

    public virtual Usuario? Usuario { get; set; }

    [Required]
    [ForeignKey("Producto")]
    public int IdProducto { get; set; }

    public virtual Producto? Producto { get; set; }

    [Required]
    public int IdMovimiento { get; set; }

    [Required]
    public bool TipoMovimiento { get; set; }

    [Required]
    public double Total { get; set; }

    [Required]
    public double PrecioUnidad { get; set; }

    [Required]
    public int StockAnterior { get; set; }

    [Required]
    public int StockActual { get; set; }
}

