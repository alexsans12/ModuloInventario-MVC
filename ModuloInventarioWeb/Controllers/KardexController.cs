using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModuloInventarioWeb.Data;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Controllers
{
    public class KardexController : Controller
    {

        private readonly IKardex _kardexData;
        private readonly IProductoData _productoData;
        private readonly IUsuarioData _usuarioData;

        public KardexController(IKardex kardexData, IProductoData productoData, IUsuarioData usuarioData)
        {
            _kardexData = kardexData;
            _productoData = productoData;
            _usuarioData = usuarioData;
        }

        public async Task<IActionResult> Index(int pg = 1)
        {
            try
            {
                IEnumerable<Kardex> kardices = await _kardexData.GetAll();

                /*if (tipo == 0)
                {
                    kardices = (IEnumerable<Kardex>)await _kardexData.GetByTipo(false);
                }
                else if (tipo == 1)
                {
                    kardices = (IEnumerable<Kardex>)await _kardexData.GetByTipo(true);
                }
                else
                {
                    kardices = await _kardexData.GetAll();

                }*/

                foreach (Kardex kardex in kardices)
                {
                    kardex.Usuario = await _usuarioData.GetUsuario(kardex.IdUsuario);
                }

                const int pageSize = 9;

                if (pg < 1) pg = 1;

                int recsCount = kardices.Count();
                var pager = new Pager(recsCount, pg, pageSize);
                int recSkip = (pg - 1) * pageSize;
                var data = kardices.Skip(recSkip).Take(pager.PageSize);

                ViewBag.Pager = pager;

                return View(data);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

