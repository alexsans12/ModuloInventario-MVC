using System;
namespace ModuloInventarioWeb.Models;

public class Usuario
{
    public int ID_Usuario { get; set; }
    public string Nombre { get; set; }
    public string Contrasena { get; set; }
    public byte[] Foto_Perfil { get; set; }
    public int ID_Rol { get; set; }
    public int ID_Empleado { get; set; }

}
