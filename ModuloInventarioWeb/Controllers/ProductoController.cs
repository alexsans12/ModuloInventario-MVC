using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModuloInventarioWeb.Data;
using ModuloInventarioWeb.Models;
using static System.Net.Mime.MediaTypeNames;

namespace ModuloInventarioWeb.Controllers;

public class ProductoController : Controller
{
    private readonly IProductoData _data;
    private readonly ICategoriaData _categoriaData;

    public ProductoController(IProductoData data, ICategoriaData categoriaData)
    {
        _data = data;
        _categoriaData = categoriaData;
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

    public  IActionResult Create()
    {
        try
        {
            List<SelectListItem> lista = ObtenerCategorias().Result;
            ViewBag.ListaCategorias = lista;
        }
        catch (Exception ex){ 
        }
        return View();
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Producto producto)
    {
        try
        {
            List<SelectListItem> lista = ObtenerCategorias().Result;
            ViewBag.ListaCategorias = lista;
            IFormFile imagen = Request.Form.Files[0];
            var filestream = imagen.OpenReadStream();
            byte[] data = new byte[filestream.Length];
            filestream.Read(data, 0, data.Length);

           

            if (ModelState.IsValid)
            {
                producto.Imagen_Producto = data;
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

    public async Task<IActionResult> Edit(int? id)
    {
        List<SelectListItem> lista = ObtenerCategorias().Result;
        ViewBag.ListaCategorias = lista;

        if (id is 0 or null)
        {
            return NotFound();
        }

        try
        {
            var obj = await _data.GetProducto((int)id);

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
            List<SelectListItem> lista = ObtenerCategorias().Result;
            ViewBag.ListaCategorias = lista;
            IFormFile imagen = Request.Form.Files[0];
            var filestream = imagen.OpenReadStream();
            byte[] data = new byte[filestream.Length];
            filestream.Read(data, 0, data.Length);

            producto.Imagen_Producto = data;
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

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _data.DeleteProducto(id);
            TempData["success"] = "Producto deleted successfully";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }



    private async Task<List<SelectListItem>> ObtenerCategorias()
    {
        IEnumerable<Categoria> listaCategorias = await _categoriaData.GetCategoria();
        List<SelectListItem> listItems = new List<SelectListItem>();

        foreach (var categoria in listaCategorias)
        {
            listItems.Add(new SelectListItem
            {
                Text = categoria.Nombre,
                Value = categoria.ID_Categoria.ToString()
            });
        }
        return listItems;
    }

    public async Task<IActionResult> View(int? id)
    {
        List<SelectListItem> lista = ObtenerCategorias().Result;
        ViewBag.ListaCategorias = lista;

        if (id is 0 or null)
        {
            return NotFound();
        }

        try
        {
            var obj = await _data.GetProducto((int)id);

            if (obj is null)
                return NotFound();

            return View(obj);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }


}
