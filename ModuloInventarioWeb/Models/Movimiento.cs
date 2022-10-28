using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModuloInventarioWeb.Models;

public class Movimiento
{
    [Key]
    public int Id { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    [Required]
    public bool TipoMovimiento { get; set; }

    [Required]
    public String Descripcion { get; set; }

    [Required]
    public double Total { get; set; }

    [Required]
    [ForeignKey("Usuario")]
    public int IdUsuario { get; set; }

    public virtual Usuario? Usuario { get; set; }

    [Required]
    public virtual List<DetalleMovimiento>? Detalles { get; set; }
}

