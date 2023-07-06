using AppAempaPasteleria.Modulos;
using AppAempaPasteleria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace AppAempaPasteleria.Controllers
{
	[Authorize]
	public class InsumoController : Controller
    {
        proveedorDAO _prove = new proveedorDAO();
        insumoDAO _insu = new insumoDAO();

        //listar
        public async Task<IActionResult> ListarInsumos(string buscar = "")
        {
            var listadoIn = _insu.listado();
            ViewBag.CANTIINSU = listadoIn.Count();
            if (string.IsNullOrEmpty(buscar))
            {
                return View(listadoIn);
            }
            else
            {
                var listaFil = listadoIn.Where(p => p.desIn.StartsWith(buscar)).ToList();
                ViewBag.CANTIINSU = listaFil.Count();
                return View(listaFil);
            }
        }

        //insertar
        public async Task<IActionResult> GuardarInsumos()
        {
            ViewBag.prove = new SelectList(_prove.listado(), "idProv", "razProv");
            return View(await Task.Run(() => new Insumo()));
        }

        [HttpPost]
        public async Task<IActionResult> GuardarInsumos(Insumo reg)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.prove = new SelectList(_prove.listado(), "idProv", "razProv");
                return View(await Task.Run(() => reg));
            }

            ViewBag.mensaje = _insu.Agregar(reg);
            ViewBag.prove = new SelectList(_prove.listado(), "idProv", "razProv");
            return View(await Task.Run(() => reg));
        }

        //editar
        public async Task<IActionResult> EditarInsumos(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {

                return RedirectToAction("ListarInsumos", "Insumo");
            }

            Insumo reg = _insu.Buscar(id);

            ViewBag.prove = new SelectList(_prove.listado(), "idProv", "razProv");
            return View(await Task.Run(() => reg));
        }

        [HttpPost]
        public async Task<IActionResult> EditarInsumos(Insumo reg)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.prove = new SelectList(_prove.listado(), "idProv", "razProv");
                return View(await Task.Run(() => reg));
            }


            ViewBag.mensaje = _insu.Actualizar(reg);
            ViewBag.insu = _insu.listado();
            ViewBag.prove = new SelectList(_prove.listado(), "idProv", "razProv");
            return View(await Task.Run(() => reg));
        }

        //eliminar

        [HttpGet]
        public async Task<IActionResult> InsumoEliminar(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("ListarInsumos", "Insumo");
            }

            Insumo reg = _insu.Buscar(id);
            return View(await Task.Run(() => reg));
        }

        // POST: Realizar eliminación
        [HttpPost]
        [ActionName("InsumoEliminar")]
        public async Task<IActionResult> InsumoEliminarConfirmar(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("ListarInsumos", "Insumo");
            }

            string mensaje = _insu.Eliminar(id);
            ViewBag.mensaje = mensaje;
            return RedirectToAction("ListarInsumos");
        }

    }
}
