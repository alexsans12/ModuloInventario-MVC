using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModuloInventarioWeb.Data;
using ModuloInventarioWeb.Models;


namespace ModuloInventarioWeb.Controllers;

public class ProductoController : Controller
{
    private readonly IProductoData _data;

    public ProductoController(IProductoData data)
    {
        _data = data;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            IEnumerable<Producto> objProductoList = await _data.GetProducto();
            return View(objProductoList);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    public async Task<IActionResult> Create()
    {
        return View();
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Producto producto)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _data.InsertProducto(producto);
                TempData["success"] = "Producto created successfully";
                return RedirectToAction("Index");
            }

            return View(producto);
        }
        catch (Exception ex)
        {
            TempData["error"] = ex.Message;
            return View(producto);
        }
    }

    public async Task<IActionResult> Edit(int? ID_Producto)
    {

        if (ID_Producto is 0 or null)
        {
            return NotFound();
        }

        try
        {
            var obj = await _data.GetProducto((int)ID_Producto);

            if (obj is null)
                return NotFound();

            return View(obj);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Producto producto)
    {
        try
        {
            await _data.UpdateProducto(producto);
            TempData["success"] = "Producto updated successfully";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["error"] = ex.Message;
            return View(producto);
        }
    }

    public async Task<IActionResult> Delete(int ID_Producto)
    {
        try
        {
            await _data.DeleteProducto(ID_Producto);
            TempData["success"] = "Producto deleted successfully";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }

}
