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
    private readonly IKardexData _kardexData;

    public MovimientoController(IMovimientoData movimientoData, IProductoData productoData, IDetalleMovimientoData detalleMovimientoData, IUsuarioData usuarioData, IKardexData kardexData)
    {
        _movimientoData = movimientoData;
        _productoData = productoData;
        _detalleMovientoData = detalleMovimientoData;
        _usuarioData = usuarioData;
        _kardexData = kardexData;
    }

    public async Task<IActionResult> Index(int pg = 1)
    {
        try
        {
            IEnumerable<Movimiento> movimientos = await _movimientoData.ObtenerTodos();

            foreach (Movimiento movimiento in movimientos)
            {
                movimiento.Usuario = await _usuarioData.GetUsuario(movimiento.IdUsuario);
            }

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

    public async Task<IActionResult> Create()
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
    public async Task<IActionResult> Create(Movimiento movimiento)
    {
        try
        {
            movimiento.IdUsuario = int.Parse(User.FindFirst("Id_Usuario").Value);

            int idMovimiento = await _movimientoData.Insertar(movimiento);

            foreach (DetalleMovimiento detalle in movimiento.Detalles)
            {
                Kardex kardex = new Kardex();

                detalle.IdMovimiento = idMovimiento;
                await _detalleMovientoData.Insertar(detalle);
                detalle.Producto = await _productoData.GetProducto(detalle.IdProducto);
                kardex.StockAnterior = detalle.Producto.Stock;
                detalle.Producto.Stock = detalle.Producto.Stock + detalle.Cantidad;
                await _productoData.UpdateProducto(detalle.Producto);

                kardex.FechaCreacion = movimiento.FechaCreacion;
                kardex.Motivo = movimiento.Descripcion;
                kardex.Cantidad = detalle.Cantidad;
                kardex.IdUsuario = movimiento.IdUsuario;
                kardex.IdProducto = detalle.IdProducto;
                kardex.IdMovimiento = movimiento.Id;
                kardex.TipoMovimiento = movimiento.TipoMovimiento;
                kardex.Total = detalle.Subtotal;
                kardex.PrecioUnidad = detalle.PrecioUnidad;
                kardex.StockActual = detalle.Producto.Stock;

                await _kardexData.InsertKardex(kardex);
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

    public async Task<IActionResult> Edit(int? id)
    {
        try
        {
            IEnumerable<Producto> productos = await _productoData.GetProducto();
            ViewBag.Productos = productos;

            List<SelectListItem> listaProductos = ObtenerListaProductos().Result;
            ViewBag.ListaProductos = listaProductos;

            Movimiento movimiento = await _movimientoData.ObtenerPorId((int)id);
            movimiento.Detalles = (List<DetalleMovimiento>?) await _detalleMovientoData.ObtenerPorMovimiento(movimiento.Id);

            foreach (DetalleMovimiento detalle in movimiento.Detalles)
            {
                detalle.Producto = await _productoData.GetProducto(detalle.IdProducto);
            }

            return View(movimiento);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Movimiento movimiento)
    {
        try
        {
            movimiento.IdUsuario = int.Parse(User.FindFirst("Id_Usuario").Value);

            Movimiento movimientoAnt = await _movimientoData.ObtenerPorId((int)movimiento.Id);
            movimientoAnt.Detalles = (List<DetalleMovimiento>?) await _detalleMovientoData.ObtenerPorMovimiento(movimientoAnt.Id);

            await _movimientoData.Actualizar(movimiento);

            foreach (DetalleMovimiento detalle in movimientoAnt.Detalles)
            {
                detalle.Producto = await _productoData.GetProducto(detalle.IdProducto);
                detalle.Producto.Stock = detalle.Producto.Stock - detalle.Cantidad;
                await _productoData.UpdateProducto(detalle.Producto);
            }

            await _detalleMovientoData.Borrar(movimiento.Id);
            await _kardexData.DeleteKardexMov(movimiento.Id);

            foreach (DetalleMovimiento detalle in movimiento.Detalles)
            {
                Kardex kardex = new Kardex();

                detalle.IdMovimiento = movimiento.Id;
                await _detalleMovientoData.Insertar(detalle);
                detalle.Producto = await _productoData.GetProducto(detalle.IdProducto);
                kardex.StockAnterior = detalle.Producto.Stock;
                detalle.Producto.Stock = detalle.Producto.Stock + detalle.Cantidad;
                await _productoData.UpdateProducto(detalle.Producto);

                kardex.FechaCreacion = movimiento.FechaCreacion;
                kardex.Motivo = movimiento.Descripcion;
                kardex.Cantidad = detalle.Cantidad;
                kardex.IdUsuario = movimiento.IdUsuario;
                kardex.IdProducto = detalle.IdProducto;
                kardex.IdMovimiento = movimiento.Id;
                kardex.TipoMovimiento = movimiento.TipoMovimiento;
                kardex.Total = detalle.Subtotal;
                kardex.PrecioUnidad = detalle.PrecioUnidad;
                kardex.StockActual = detalle.Producto.Stock;

                await _kardexData.InsertKardex(kardex);
            }

            TempData["success"] = "El movimiento se a actualizado exitosamente";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["error"] = ex.Message;
            return View(movimiento);
        }
    }

    public async Task<IActionResult> Delete(int Id)
    {
        try
        {
            Movimiento movimiento = await _movimientoData.ObtenerPorId((int)Id);

            if(movimiento.TipoMovimiento)
            {
                TempData["error"] = "Solo pueden eliminarse movimientos de entrada";
                return RedirectToAction("Index");
            }

            movimiento.Detalles = (List<DetalleMovimiento>?)await _detalleMovientoData.ObtenerPorMovimiento(movimiento.Id);

            foreach (DetalleMovimiento detalle in movimiento.Detalles)
            {
                detalle.Producto = await _productoData.GetProducto(detalle.IdProducto);
                detalle.Producto.Stock = detalle.Producto.Stock - detalle.Cantidad;
                await _productoData.UpdateProducto(detalle.Producto);
            }

            await _kardexData.DeleteKardexMov(movimiento.Id);
            await _detalleMovientoData.Borrar(movimiento.Id);
            await _movimientoData.Borrar(movimiento.Id);

            TempData["success"] = "Movimiento de entrada eliminado exitosamente";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }

    public async Task<IActionResult> View(int? id)
    {
        try
        {
            IEnumerable<Producto> productos = await _productoData.GetProducto();
            ViewBag.Productos = productos;

            List<SelectListItem> listaProductos = ObtenerListaProductos().Result;
            ViewBag.ListaProductos = listaProductos;

            Movimiento movimiento = await _movimientoData.ObtenerPorId((int)id);
            movimiento.Detalles = (List<DetalleMovimiento>?)await _detalleMovientoData.ObtenerPorMovimiento(movimiento.Id);

            movimiento.Usuario = await _usuarioData.GetUsuario(movimiento.IdUsuario);

            return View(movimiento);
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
            listItems.Add(new SelectListItem {
                Text = producto.Nombre,
                Value = producto.ID_Producto.ToString()
            });
        }
        return listItems;
    }

}
