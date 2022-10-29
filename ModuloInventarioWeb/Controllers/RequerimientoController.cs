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
    public class RequerimientoController : Controller
    {
        private readonly IRequerimientoData _requerimientoData;
        private readonly IDetalleRequerimientoData _detalleRequerimientoData;
        private readonly IProductoData _productoData;
        private readonly IUsuarioData _usuarioData;
        private readonly IMovimientoData _movimientoData;
        private readonly IDetalleMovimientoData _detalleMovientoData;
        private readonly IKardexData _kardexData;

        public RequerimientoController(IRequerimientoData requerimientoData, IDetalleRequerimientoData detalleRequerimientoData, IProductoData productoData, IUsuarioData usuarioData, IMovimientoData movimientoData, IDetalleMovimientoData detalleMovimientoData, IKardexData kardexData)
        {
            _requerimientoData = requerimientoData;
            _detalleRequerimientoData = detalleRequerimientoData;
            _productoData = productoData;
            _usuarioData = usuarioData;
            _movimientoData = movimientoData;
            _detalleMovientoData = detalleMovimientoData;
            _kardexData = kardexData;
        }

        public async Task<IActionResult> Index(int pg = 1)
        {
            try
            {
                IEnumerable<Requerimiento> requerimientos = await _requerimientoData.ObtenerTodos();

                foreach (Requerimiento requerimiento in requerimientos)
                {
                    requerimiento.UsuarioIngreso = await _usuarioData.GetUsuario(requerimiento.IdUsuarioIngreso);
                }

                const int pageSize = 9;

                if (pg < 1) pg = 1;

                int recsCount = requerimientos.Count();
                var pager = new Pager(recsCount, pg, pageSize);
                int recSkip = (pg - 1) * pageSize;
                var data = requerimientos.Skip(recSkip).Take(pager.PageSize);

                ViewBag.Pager = pager;

                return View(data);
            }
            catch(Exception ex)
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

                Requerimiento requerimiento = new Requerimiento();
                requerimiento.Detalles = new List<DetalleRequerimiento>();
                requerimiento.Detalles.Add(new DetalleRequerimiento());

                return View(requerimiento);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Requerimiento requerimiento)
        {
            try
            {
                requerimiento.IdUsuarioIngreso = int.Parse(User.FindFirst("Id_Usuario").Value);
                requerimiento.Estado = "Pendiente";

                int idRequerimiento = await _requerimientoData.Insertar(requerimiento);

                foreach (DetalleRequerimiento detalle in requerimiento.Detalles)
                {
                    detalle.IdRequerimiento = idRequerimiento;
                    await _detalleRequerimientoData.Insertar(detalle);
                }

                TempData["success"] = "Requerimiento creado exitosamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index");
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

                Requerimiento requerimiento = await _requerimientoData.ObtenerPorId((int)id);
                requerimiento.Detalles = (List<DetalleRequerimiento>?) await _detalleRequerimientoData.ObtenerPorRequerimiento(requerimiento.Id);

                foreach (DetalleRequerimiento detalle in requerimiento.Detalles)
                {
                    detalle.Producto = await _productoData.GetProducto(detalle.IdProducto);
                }

                return View(requerimiento);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Requerimiento requerimiento)
        {
            try
            {
                requerimiento.IdUsuarioIngreso = int.Parse(User.FindFirst("Id_Usuario").Value);

                Requerimiento requerimientoAnt = await _requerimientoData.ObtenerPorId((int)requerimiento.Id);
                requerimientoAnt.Detalles = (List<DetalleRequerimiento>?) await _detalleRequerimientoData.ObtenerPorRequerimiento(requerimientoAnt.Id);

                await _requerimientoData.Actualizar(requerimiento);

                await _detalleRequerimientoData.Borrar(requerimiento.Id);

                foreach (DetalleRequerimiento detalle in requerimiento.Detalles)
                {
                    detalle.IdRequerimiento = requerimiento.Id;
                    await _detalleRequerimientoData.Insertar(detalle);
                }

                TempData["success"] = "El requerimiento se a actualizado exitosamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                Requerimiento requerimiento = await _requerimientoData.ObtenerPorId((int)Id);
                requerimiento.Detalles = (List<DetalleRequerimiento>?)await _detalleRequerimientoData.ObtenerPorRequerimiento(requerimiento.Id);

                if(requerimiento.IdUsuarioAutorizo != null)
                {
                    foreach (DetalleRequerimiento detalle in requerimiento.Detalles)
                    {
                        detalle.Producto = await _productoData.GetProducto(detalle.IdProducto);
                        detalle.Producto.Stock = detalle.Producto.Stock + detalle.Cantidad;
                        await _productoData.UpdateProducto(detalle.Producto);
                    }
                }

                if(requerimiento.IdUsuarioAutorizo != null)
                {
                    Kardex kardex = await _kardexData.GetByIdRequerimiento(requerimiento.Id);
                    await _detalleMovientoData.Borrar(kardex.IdMovimiento);
                    await _movimientoData.Borrar(kardex.IdMovimiento);
                    await _kardexData.DeleteKardexReq(requerimiento.Id);
                }


                await _detalleRequerimientoData.Borrar(requerimiento.Id);
                await _requerimientoData.Borrar(requerimiento.Id);

                TempData["success"] = "Requerimiento eliminado exitosamente";
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

                Requerimiento requerimiento = await _requerimientoData.ObtenerPorId((int)id);
                requerimiento.Detalles = (List<DetalleRequerimiento>?)await _detalleRequerimientoData.ObtenerPorRequerimiento(requerimiento.Id);

                requerimiento.UsuarioIngreso = await _usuarioData.GetUsuario(requerimiento.IdUsuarioIngreso);

                if(requerimiento.IdUsuarioAutorizo != null)
                {
                    requerimiento.UsuarioAutorizo = await _usuarioData.GetUsuario((int)requerimiento.IdUsuarioAutorizo);
                }

                return View(requerimiento);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        public async Task<IActionResult> Authorize(int? id)
        {
            try
            {
                Requerimiento requerimiento = await _requerimientoData.ObtenerPorId((int)id);
                requerimiento.Detalles = (List<DetalleRequerimiento>?)await _detalleRequerimientoData.ObtenerPorRequerimiento(requerimiento.Id);

                requerimiento.IdUsuarioAutorizo = int.Parse(User.FindFirst("Id_Usuario").Value);
                requerimiento.Estado = "Autorizado";

                await _requerimientoData.Autorizar(requerimiento);

                foreach (DetalleRequerimiento detalle in requerimiento.Detalles)
                {
                    detalle.Producto = await _productoData.GetProducto(detalle.IdProducto);
                    detalle.Producto.Stock = detalle.Producto.Stock - detalle.Cantidad;
                    await _productoData.UpdateProducto(detalle.Producto);
                }

                Movimiento movimiento = new Movimiento();
                movimiento.IdUsuario = requerimiento.IdUsuarioIngreso;
                movimiento.TipoMovimiento = true;
                movimiento.Descripcion = requerimiento.Motivo;
                movimiento.Total = requerimiento.Total;

                int idMovimiento = await _movimientoData.Insertar(movimiento);

                movimiento.Detalles = new List<DetalleMovimiento>();

                foreach (DetalleRequerimiento detalle in requerimiento.Detalles)
                {
                    Kardex kardex = new Kardex();

                    DetalleMovimiento detalleMovimiento = new DetalleMovimiento();
                    detalleMovimiento.IdMovimiento = idMovimiento;
                    detalleMovimiento.IdProducto = detalle.IdProducto;
                    detalleMovimiento.Cantidad = detalle.Cantidad;
                    detalleMovimiento.PrecioUnidad = detalle.PrecioUnidad;
                    detalleMovimiento.Subtotal = detalle.Subtotal;
                    await _detalleMovientoData.Insertar(detalleMovimiento);

                    detalle.Producto = await _productoData.GetProducto(detalle.IdProducto);
                    kardex.StockAnterior = detalle.Producto.Stock + detalle.Cantidad;
                    kardex.FechaCreacion = movimiento.FechaCreacion;
                    kardex.Motivo = movimiento.Descripcion;
                    kardex.Cantidad = detalle.Cantidad;
                    kardex.IdUsuario = movimiento.IdUsuario;
                    kardex.IdProducto = detalle.IdProducto;
                    kardex.IdMovimiento = idMovimiento;
                    kardex.IdRequerimiento = requerimiento.Id;
                    kardex.TipoMovimiento = movimiento.TipoMovimiento;
                    kardex.Total = detalle.Subtotal;
                    kardex.PrecioUnidad = detalle.PrecioUnidad;
                    kardex.StockActual = detalle.Producto.Stock;

                    await _kardexData.InsertKardex(kardex);
                }

                TempData["success"] = "Requerimiento autorizado exitosamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        private async Task<List<SelectListItem>> ObtenerListaProductos()
        {
            IEnumerable<Producto> listaProductos = await _productoData.GetProducto();
            List<SelectListItem> listItems = new List<SelectListItem>();

            foreach (var producto in listaProductos)
            {
                if (producto.Stock > 0)
                {
                    listItems.Add(new SelectListItem
                    {
                        Text = producto.Nombre,
                        Value = producto.ID_Producto.ToString()
                    });
                }
            }
            return listItems;
        }
    }
}

