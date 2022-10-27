using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModuloInventarioWeb.Data;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Controllers
{
    public class RequerimientoController : Controller
    {
        private readonly IRequerimientoData _requerimientoData;
        private readonly IProductoData _productoData;
        private readonly IUsuarioData _usuarioData;

        public RequerimientoController(IRequerimientoData requerimientoData,IProductoData productoData, IUsuarioData usuarioData)
        {
            _requerimientoData = requerimientoData;
            _productoData = productoData;
            _usuarioData = usuarioData;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<Requerimiento> requerimientos = await _requerimientoData.ObtenerTodos();

                return View(requerimientos);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
    }
}

