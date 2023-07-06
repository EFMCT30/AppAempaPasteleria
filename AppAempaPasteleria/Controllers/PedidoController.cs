using AppAempaPasteleria.Models;
using AppAempaPasteleria.Modulos;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AppAempaPasteleria.Controllers
{
    [Authorize]
    public class PedidoController : Controller
    {
        pedidoDAO _pe = new pedidoDAO();

        //listarPedido
        public async Task<IActionResult> ListarPedido(string buscar = "")
        {
            var listadoPe = _pe.listado();
            ViewBag.Cantidad = listadoPe.Count();
            if (string.IsNullOrEmpty(buscar))
            {
                return View(listadoPe);
            }
            else
            {
                var listaFil = listadoPe.Where(p => p.EstadoPed.StartsWith(buscar)).ToList();
                ViewBag.Cantidad = listaFil.Count();
                return View(listaFil);
            }
        }

        public async Task<IActionResult> EditarPedido(int id)
        {
            Pedido reg = _pe.Buscar(id);

            return View(await Task.Run(() => reg));
        }

        [HttpPost]
        public async Task<IActionResult> EditarPedido(Pedido reg)
        {
            
            ViewBag.mensaje = _pe.Actualizar(reg);
            ViewBag.pedidos = _pe.listado();
            return View(await Task.Run(() => reg));
        }

        //eliminar
        [HttpGet]
        public async Task<IActionResult> PedidoEliminar(int id)
        {
            Pedido reg = _pe.Buscar(id);
            return View(await Task.Run(() => reg));
        }

        // POST: Realizar eliminación
        [HttpPost]
        [ActionName("PedidoEliminar")]
        public async Task<IActionResult> PedidoEliminarConfirmar(int id)
        {
            string mensaje = _pe.Eliminar(id);
            ViewBag.mensaje = mensaje;
            return RedirectToAction("ListarPedido");
        }

    }
}
