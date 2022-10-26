﻿using System;
using System.Data.SqlTypes;

namespace ModuloInventarioWeb.Models
{
    public class Producto
    {
        public int ID_Producto { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int IDCategoria { get; set; }
        public int Stock { get; set; }
        public double Precio_Entrada { get; set; }
        public byte[]? Imagen_Producto { get; set; }


    }
}
