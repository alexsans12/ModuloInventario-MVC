using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModuloInventarioWeb.Data;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Controllers
{
    [Authorize]
    public class KardexController : Controller
    {

        private readonly IKardexData _kardexData;
        private readonly IProductoData _productoData;
        private readonly IUsuarioData _usuarioData;

        public KardexController(IKardexData kardexData, IProductoData productoData, IUsuarioData usuarioData)
        {
            _kardexData = kardexData;
            _productoData = productoData;
            _usuarioData = usuarioData;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                List<SelectListItem> listaProductos = ObtenerListaProductos().Result;
                ViewBag.ListaProductos = listaProductos;

                return View();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(Kardex kardex)
        {
            try
            {
                int IdProducto = kardex.IdProducto;
                IEnumerable<Kardex> kardices = await _kardexData.GetByProducto(IdProducto);

                Producto producto = await _productoData.GetProducto(IdProducto);
                ViewBag.Producto = producto;

                List<SelectListItem> listaProductos = ObtenerListaProductos().Result;
                ViewBag.ListaProductos = listaProductos;

                foreach (Kardex kardexp in kardices)
                {
                    kardexp.Usuario = await _usuarioData.GetUsuario(kardex.IdUsuario);
                    kardexp.Producto = await _productoData.GetProducto(kardex.IdProducto);
                }

                return View(kardices);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        private async Task<List<SelectListItem>> ObtenerListaProductos()
        {
            IEnumerable<Producto> listaProductos = await _productoData.GetProducto();
            List<SelectListItem> listItems = new List<SelectListItem>();

            foreach (var producto in listaProductos)
            {
                listItems.Add(new SelectListItem
                {
                    Text = producto.Nombre,
                    Value = producto.ID_Producto.ToString()
                });
            }
            return listItems;
        }
    }
}

