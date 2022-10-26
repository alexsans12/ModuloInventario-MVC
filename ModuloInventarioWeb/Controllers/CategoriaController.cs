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

        public async Task<IActionResult> Index(int pg = 1)
        {
            try
            {
                IEnumerable<Categoria> objCategoriaList = await _data.GetCategoria();
                const int pageSize = 9;

                if (pg < 1) pg = 1;

                int recsCount = objCategoriaList.Count();
                var pager = new Pager(recsCount, pg, pageSize);
                int recSkip = (pg - 1) * pageSize;
                var data = objCategoriaList.Skip(recSkip).Take(pager.PageSize);

                ViewBag.Pager = pager;

            return View(data);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        public  IActionResult Create()
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
                    TempData["success"] = "Categoria created successfully";
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

    public async Task<IActionResult> Edit(int? id)
    {

        if (id is 0 or null)
        {
            return NotFound();
        }

        try
        {
            var obj = await _data.GetCategoria((int)id);

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

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _data.DeleteCategoria(id);
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
