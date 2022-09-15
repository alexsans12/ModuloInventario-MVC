using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModuloInventarioWeb.Data;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Controllers;

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

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var obj = await _data.GetCategory(id);
            return View(obj);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }

    }
}

