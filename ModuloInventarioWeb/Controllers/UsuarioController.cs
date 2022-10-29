using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModuloInventarioWeb.Data;
using ModuloInventarioWeb.Helpers;
using ModuloInventarioWeb.Models;
using static System.Net.Mime.MediaTypeNames;
namespace ModuloInventarioWeb.Controllers;

[Authorize]
public class UsuarioController : Controller
{
    private readonly IUsuarioData _data;

    public UsuarioController(IUsuarioData data)
    {
        _data = data;
    }

    public async Task<IActionResult> Index(int pg = 1)
    {
        try
        {
            IEnumerable<Usuario> objUsuarioList = await _data.GetUsuario();

            const int pageSize = 5;

            if (pg < 1) pg = 1;

            int recsCount = objUsuarioList.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = objUsuarioList.Skip(recSkip).Take(pager.PageSize);

            ViewBag.Pager = pager;
            return View(data);
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
    public async Task<IActionResult> Create(Usuario usuario, String password)
    {
        try
        {
            IFormFile imagen = Request.Form.Files[0];
            var filestream = imagen.OpenReadStream();
            byte[] data = new byte[filestream.Length];
            filestream.Read(data, 0, data.Length);

            string salt = usuario.Nombre.ToUpper();
            byte[] passTemporal = HelperCryptography.EncriptarPassword(password, salt);

            if (ModelState.IsValid)
            {
                usuario.Foto_Perfil = data;
                usuario.Contrasena = passTemporal;
                await _data.InsertUsuario(usuario);
                TempData["success"] = "Usuario creado exitosamente";
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
    public async Task<IActionResult> Edit(Usuario usuario, string password)
    {
        try
        {
            if (Request.Form.Files.Count() > 0)
            {
                IFormFile imagen = Request.Form.Files[0];
                var filestream = imagen.OpenReadStream();
                byte[] data = new byte[filestream.Length];
                filestream.Read(data, 0, data.Length);
                usuario.Foto_Perfil = data;
            } else
            {
                var obj = await _data.GetUsuario(usuario.ID_Usuario);
                usuario.Foto_Perfil = obj.Foto_Perfil;
            }

            string salt = usuario.Nombre.ToUpper();
            byte[] passTemporal = HelperCryptography.EncriptarPassword(password, salt);

            usuario.Contrasena = passTemporal;

            await _data.UpdateUsuario(usuario);

            TempData["success"] = "Usuario actualizado exitosamente";
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
            TempData["success"] = "Usuario eliminado exitosamente";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }
}
