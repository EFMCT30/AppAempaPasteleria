using AppAempaPasteleria.Modulos;
using AppAempaPasteleria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace AppAempaPasteleria.Controllers
{
	[Authorize]
	public class ProveedorController : Controller
    {
        proveedorDAO _prove = new proveedorDAO();
        paisDAO _pais = new paisDAO();

        //listar
        public async Task<IActionResult> ListarProveedores(string buscar = "")
        {
            var listadoPro = _prove.listado();
            ViewBag.CANTIPROVE = listadoPro.Count();
            if (string.IsNullOrEmpty(buscar))
            {
                return View(listadoPro);
            }
            else
            {
                var listaFil = listadoPro.Where(p => p.razProv.StartsWith(buscar)).ToList();
                ViewBag.CANTIPROVE = listaFil.Count();
                return View(listaFil);
            }
        }

        //insertar
        public async Task<IActionResult> GuardarProveedores()
        {
            ViewBag.paises = new SelectList(_pais.listado(), "idpais", "nombrepais");
            return View(await Task.Run(() => new Proveedor()));
        }

        [HttpPost]
        public async Task<IActionResult> GuardarProveedores(Proveedor reg)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.paises = new SelectList(_pais.listado(), "idpais", "nombrepais");
                return View(await Task.Run(() => reg));
            }

            ViewBag.mensaje = _prove.Agregar(reg);
            ViewBag.paises = new SelectList(_pais.listado(), "idpais", "nombrepais");
            return View(await Task.Run(() => reg));
        }

        //editar
        public async Task<IActionResult> EditarProveedores(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {

                return RedirectToAction("ListarProveedores", "Proveedor");
            }

            Proveedor reg = _prove.Buscar(id);

            ViewBag.paises = new SelectList(_pais.listado(), "idpais", "nombrepais", reg.idPais);
            return View(await Task.Run(() => reg));
        }

        [HttpPost]
        public async Task<IActionResult> EditarProveedores(Proveedor reg)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.paises = new SelectList(_pais.listado(), "idpais", "nombrepais", reg.idPais);
                return View(await Task.Run(() => reg));
            }


            ViewBag.mensaje = _prove.Actualizar(reg);
            ViewBag.proveedores = _prove.listado();
            ViewBag.paises = new SelectList(_pais.listado(), "idpais", "nombrepais", reg.idPais);
            return View(await Task.Run(() => reg));
        }

        //eliminar

        [HttpGet]
        public async Task<IActionResult> ProveedorEliminar(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("ListarProveedores", "Proveedor");
            }

            Proveedor reg = _prove.Buscar(id);
            return View(await Task.Run(() => reg));
        }

        // POST: Realizar eliminación
        [HttpPost]
        [ActionName("ProveedorEliminar")]
        public async Task<IActionResult> ProveedorEliminarConfirmar(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("ListarProveedores", "Proveedor");
            }

            string mensaje = _prove.Eliminar(id);
            ViewBag.mensaje = mensaje;
            return RedirectToAction("ListarProveedores");
        }
    }
}
