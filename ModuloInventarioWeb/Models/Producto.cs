using System;
using System.Data.SqlTypes;

namespace ModuloInventarioWeb.Models
{
    public class Producto
    {
        public int ID_Producto { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public DateTime Fecha_Vencimiento { get; set; } = DateTime.Now;
        public int ID_Categoria { get; set; }
        public int Stock { get; set; }
        public int Stock_Min { get; set; }
        public double Precio_Entrada { get; set; }
        public double Precio_Salida { get; set; }
        public byte[] Imagen_Producto { get; set; }


    }
}
