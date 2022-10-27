using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModuloInventarioWeb.Models;

public class Requerimiento
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Usuario")]
    public int IdUsuarioIngreso { get; set; }

    public virtual Usuario? UsuarioIngreso { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime FechaIngreso { get; set; } = DateTime.Now.Date;

    [ForeignKey("Usuario")]
    public int? IdUsuarioAutorizo { get; set; }

    public virtual Usuario? UsuarioAutorizo { get; set; }

    [DataType(DataType.Date)]
    public DateTime? FechaAutorizo { get; set; } = DateTime.Now.Date;

    [Required]
    public String Motivo { get; set; }

    [Required]
    public double Total { get; set; }

    [Required]
    public String Estado { get; set; }

    public virtual List<DetalleRequerimiento>? Detalles { get; set; }
}