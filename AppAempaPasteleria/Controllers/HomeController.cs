using AppAempaPasteleria.Models;
using AppAempaPasteleria.Modulos;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace AppAempaPasteleria.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        dashboardDAO _da = new dashboardDAO();

        
        public IActionResult Panel()
        {
            return View();
        }

        public IActionResult ResumenVenta()
        {
            var ListadoPedidos = _da.ObtenerResumenVenta();
            return StatusCode(StatusCodes.Status200OK, ListadoPedidos);
        }

        public IActionResult ResumenProducto()
        {
            var ListadoProductos = _da.ObtenerResumenProductos();
            return StatusCode(StatusCodes.Status200OK, ListadoProductos);
        }

        public IActionResult ResumenVentaXCli()
        {
            var ListadoCli = _da.ObtenerResumenVentaxCli();
            return StatusCode(StatusCodes.Status200OK, ListadoCli);
        }

    }
}