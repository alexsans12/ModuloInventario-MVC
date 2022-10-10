using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModuloInventarioWeb.Data;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Controllers;

[Authorize]
public class CategoryController : Controller
{
    private readonly ICategoryData _data;

    public CategoryController(ICategoryData data)
    {
        _data = data;
    }


    // GET: /<controller>/
    public async Task<IActionResult> Index()
    {
        try
        {
            IEnumerable<Category> objCategoryList = await _data.GetCategories();
            return View(objCategoryList);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }

    }

    public IActionResult Create()
    {
        return View();
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _data.InsertCategory(category);
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }

            return View(category);
        }
        catch (Exception ex)
        {
            TempData["error"] = ex.Message;
            return View(category);
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
            var obj = await _data.GetCategory((int)id);
            
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
    public async Task<IActionResult> Edit(Category category)
    {
        try
        {
            await _data.UpdateCategory(category);
            TempData["success"] = "Category updated successfully";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["error"] = ex.Message;
            return View(category);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _data.DeleteCategory(id);
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

