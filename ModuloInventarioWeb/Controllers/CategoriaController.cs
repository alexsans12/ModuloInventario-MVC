using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModuloInventarioWeb.Data;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Controllers;

    public class CategoriaController : Controller
    {
        private readonly ICategoriaData _data;

        public CategoriaController(ICategoriaData data)
        {
            _data = data;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<Categoria> objCategoriaList = await _data.GetCategoria();
                return View(objCategoriaList);
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
        public async Task<IActionResult> Create(Categoria categoria)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _data.InsertCategoria(categoria);
                    TempData["success"] = "Category created successfully";
                    return RedirectToAction("Index");
                }

                return View(categoria);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View(categoria);
            }
        }

    public async Task<IActionResult> Edit(int? ID_Categoria)
    {

        if (ID_Categoria is 0 or null)
        {
            return NotFound();
        }

        try
        {
            var obj = await _data.GetCategoria((int)ID_Categoria);

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
    public async Task<IActionResult> Edit(Categoria categoria)
    {
        try
        {
            await _data.UpdateCategoria(categoria);
            TempData["success"] = "Category updated successfully";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["error"] = ex.Message;
            return View(categoria);
        }
    }

    public async Task<IActionResult> Delete(int ID_Categoria)
    {
        try
        {
            await _data.DeleteCategoria(ID_Categoria);
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }

}
