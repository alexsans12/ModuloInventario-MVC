using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ModuloInventarioWeb.Controllers;

public class MovimientoController : Controller
{
    // GET: /<controller>/
    public IActionResult Index()
    {
        return View();
    }
}
