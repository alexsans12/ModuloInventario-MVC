using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModuloInventarioWeb.Data;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Controllers;

public class RolController : Controller
{
    private readonly IRolData _data;

    public RolController(IRolData data)
    {
        _data = data;
    }
    public async Task<IActionResult> Index()
    {
        try
        {
            IEnumerable<Rol> objRolList = await _data.GetRol();
            return View(objRolList);
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
    public async Task<IActionResult> Create(Rol rol)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _data.InsertRol(rol);
                TempData["success"] = "Rol created successfully";
                return RedirectToAction("Index");
            }

            return View(rol);
        }
        catch (Exception ex)
        {
            TempData["error"] = ex.Message;
            return View(rol);
        }
    }

    public async Task<IActionResult> Edit(int? ID_Rol)
    {

        if (ID_Rol is 0 or null)
        {
            return NotFound();
        }

        try
        {
            var obj = await _data.GetRol((int)ID_Rol);

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
    public async Task<IActionResult> Edit(Rol rol)
    {
        try
        {
            await _data.UpdateRol(rol);
            TempData["success"] = "Rol updated successfully";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["error"] = ex.Message;
            return View(rol);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _data.DeleteRol(id);
            TempData["success"] = "Rol deleted successfully";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }

}
