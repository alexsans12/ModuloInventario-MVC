using System;
using System.ComponentModel.DataAnnotations;

namespace ModuloInventarioWeb.Models;

public class Usuario
{
    [Key]
    public int ID_Usuario { get; set; }
    public string Nombre { get; set; }
    public byte[]? Contrasena { get; set; }
    public byte[]? Foto_Perfil { get; set; }
    public int ID_Empleado { get; set; }

}
