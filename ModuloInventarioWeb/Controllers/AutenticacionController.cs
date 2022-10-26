using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ModuloInventarioWeb.Data;
using ModuloInventarioWeb.Helpers;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Controllers;

public class AutenticacionController : Controller
{
    private readonly IUsuarioData _usuarioData;

    public AutenticacionController(IUsuarioData usuarioData)
    {
        _usuarioData = usuarioData;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(String nombre, String password)
    {
        Usuario usuario = Login(nombre, password).Result;

        if (usuario != null)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim("Id_Usuario", ""+usuario.Id_Usuario)
            };

            claims.Add(new Claim(ClaimTypes.Role, "Admin"));

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index","Home");

        }
        else {
            return View();
        }

    }

    public async Task<IActionResult> Salir()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index");
    }

    public async Task<Usuario> Login(String usuario, String password)
    {
        Usuario data = await _usuarioData.ObtenerPorNombre(usuario);

        if (data == null)
            return null;
        else
        {
            byte[] passUsuario = data.Contrasena;
            string salt = data.Nombre.ToUpper();
            byte[] passTemporal = HelperCryptography.EncriptarPassword(password, salt);

            //data.Contrasena = passTemporal;
            //await _usuarioData.UpdateUsuario(data);

            bool respuesta = HelperCryptography.compareArrays(passUsuario, passTemporal);

            if (respuesta == true)
                return data;
            else
                //Contraseña incorrecta
                return null;
        }
    }
}

