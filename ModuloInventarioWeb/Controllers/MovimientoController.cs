using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModuloInventarioWeb.Data;
using ModuloInventarioWeb.Models;

namespace ModuloInventarioWeb.Controllers;

[Authorize]
public class MovimientoController : Controller
{
    private readonly IMovimientoData _movimientoData;
    private readonly IUsuarioData _usuarioData;

    public MovimientoController(IMovimientoData movimientoData, IUsuarioData usuarioData)
    {
        _movimientoData = movimientoData;
        _usuarioData = usuarioData;
    }


    // GET: /<controller>/
    public async Task<IActionResult> Index()
    {
        try
        {
            IEnumerable <Movimiento> objMovimientoList = await _movimientoData.ObtenerTodos();
            foreach (var obj in objMovimientoList)
            {
                Usuario? usuario = await _usuarioData.GetUsuario(obj.IdUsuario);
                obj.Usuario = usuario;
            }
            return View(objMovimientoList);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    public IActionResult Crear()
    {
        return View();
    }
}
