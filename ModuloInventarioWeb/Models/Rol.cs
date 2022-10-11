using System;
namespace ModuloInventarioWeb.Models;

public class Rol
{
    public int ID_Rol { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public Boolean Ingreso_Requerimiento { get; set; }
    public Boolean Autorizar_Requerimineto { get; set; }
    public Boolean Borrar_Requerimiento { get; set; }
    public Boolean Ingreso_Movimiento { get; set; }



}
