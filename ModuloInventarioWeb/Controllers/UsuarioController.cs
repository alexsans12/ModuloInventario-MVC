using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModuloInventarioWeb.Data;
using ModuloInventarioWeb.Models;


namespace ModuloInventarioWeb.Controllers;

public class UsuarioController : Controller
{
    private readonly IUsuarioData _data;

    public UsuarioController(IUsuarioData data)
    {
        _data = data;
    }
    public async Task<IActionResult> Index()
    {
        try
        {
            IEnumerable<Usuario> objUsuarioList = await _data.GetUsuario();
            return View(objUsuarioList);
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
    public async Task<IActionResult> Create(Usuario usuario)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _data.InsertUsuario(usuario);
                TempData["success"] = "Usuario created successfully";
                return RedirectToAction("Index");
            }

            return View(usuario);
        }
        catch (Exception ex)
        {
            TempData["error"] = ex.Message;
            return View(usuario);
        }
    }

    public async Task<IActionResult> Edit(int? ID_Usuario)
    {

        if (ID_Usuario is 0 or null)
        {
            return NotFound();
        }

        try
        {
            var obj = await _data.GetUsuario((int)ID_Usuario);

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
    public async Task<IActionResult> Edit(Usuario usuario)
    {
        try
        {
            await _data.UpdateUsuario(usuario);
            TempData["success"] = "Usuario updated successfully";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["error"] = ex.Message;
            return View(usuario);
        }
    }

    public async Task<IActionResult> Delete(Usuario usuario)
    {
        try
        {
            await _data.DeleteUsuario(usuario);
            TempData["success"] = "Usuario deleted successfully";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }
}
