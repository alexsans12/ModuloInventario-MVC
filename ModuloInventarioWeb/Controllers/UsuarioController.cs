using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModuloInventarioWeb.Data;
using ModuloInventarioWeb.Models;
using static System.Net.Mime.MediaTypeNames;
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
    public IActionResult Create()
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
            IFormFile imagen = Request.Form.Files[0];
            var filestream = imagen.OpenReadStream();
            byte[] data = new byte[filestream.Length];
            filestream.Read(data, 0, data.Length);
        
            if (ModelState.IsValid)
            {
                usuario.Foto_Perfil = data;
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

    public async Task<IActionResult> Edit(int? id)
    {

        if (id is 0 or null)
        {
            return NotFound();
        }

        try
        {
           
            var obj = await _data.GetUsuario((int)id);

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
            IFormFile imagen = Request.Form.Files[0];
            var filestream = imagen.OpenReadStream();
            byte[] data = new byte[filestream.Length];
            filestream.Read(data, 0, data.Length);

            usuario.Foto_Perfil = data;
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

    public async Task<IActionResult> Delete(int id)
    {
        try
        {


            await _data.DeleteUsuario(id);
            TempData["success"] = "Usuario deleted successfully";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }

    public async Task<IActionResult> ObtenerImagen(int id)
    {
        Usuario usuario = await _data.GetUsuario(id);

        byte[] imagen = usuario.Foto_Perfil;

        MemoryStream memoryStream = new MemoryStream(imagen);
        return null;
    }
}
