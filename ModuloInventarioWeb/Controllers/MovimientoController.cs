using System.Dynamic;
using System.Security.Claims;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModuloInventarioWeb.Data;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Controllers;

[Authorize]
public class MovimientoController : Controller
{
    private readonly IMovimientoData _movimientoData;
    private readonly IDetalleMovimientoData _detalleMovientoData;
    private readonly IProductoData _productoData;
    private readonly IUsuarioData _usuarioData;

    public MovimientoController(IMovimientoData movimientoData, IProductoData productoData, IDetalleMovimientoData detalleMovimientoData, IUsuarioData usuarioData)
    {
        _movimientoData = movimientoData;
        _productoData = productoData;
        _detalleMovientoData = detalleMovimientoData;
        _usuarioData = usuarioData;
    }

    public async Task<IActionResult> Index(int pg = 1)
    {
        try
        {
            IEnumerable<Movimiento> movimientos = await _movimientoData.ObtenerTodos();

            const int pageSize = 9;

            if(pg < 1) pg = 1;

            int recsCount = movimientos.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = movimientos.Skip(recSkip).Take(pager.PageSize);

            ViewBag.Pager = pager;

            return View(data);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    public async Task<IActionResult> Crear()
    {
        try
        {
            IEnumerable<Producto> productos = await _productoData.GetProducto();
            ViewBag.Productos = productos;

            List<SelectListItem> listaProductos = ObtenerListaProductos().Result;
            ViewBag.ListaProductos = listaProductos;

            Movimiento movimiento = new Movimiento();
            movimiento.Detalles = new List<DetalleMovimiento>();
            movimiento.Detalles.Add(new DetalleMovimiento());

            return View(movimiento);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Movimiento movimiento)
    {
        try
        {
            movimiento.IdUsuario = int.Parse(User.FindFirst("Id_Usuario").Value);

            int idMovimiento = await _movimientoData.Insertar(movimiento);

            foreach (DetalleMovimiento detalle in movimiento.Detalles)
            {
                detalle.IdMovimiento = idMovimiento;
                await _detalleMovientoData.Insertar(detalle);
            }

            TempData["success"] = "Movimiento de entrada registrado exitosamente";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["error"] = ex.Message;
            return View(movimiento);
        }
    }

    private async Task<List<SelectListItem>> ObtenerListaProductos()
    {
        IEnumerable<Producto> listaProductos = await _productoData.GetProducto();
        List<SelectListItem> listItems = new List<SelectListItem>();

        foreach (var producto in listaProductos)
        {
            listItems.Add(new SelectListItem {
                Text = producto.Nombre,
                Value = producto.ID_Producto.ToString()
            });
        }
        return listItems;
    }

}
