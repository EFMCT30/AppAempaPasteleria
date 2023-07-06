using AppAempaPasteleria.Modulos;
using AppAempaPasteleria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace AppAempaPasteleria.Controllers
{
	[Authorize]
	public class ClienteController : Controller
    {
        clienteDAO _clientes = new clienteDAO();
        paisDAO _pais = new paisDAO();

        //LISTAR

        public async Task<IActionResult> ListarClientes(string buscar = "")
        {
            var listadoCli= _clientes.listado();
            ViewBag.CANTIDADCLI = listadoCli.Count();
            if (string.IsNullOrEmpty(buscar))
            {
                return View(listadoCli);
            }
            else
            {
                var listaFil = listadoCli.Where(p => p.nomCli.StartsWith(buscar)).ToList();
                ViewBag.CANTIDADCLI = listaFil.Count();
                return View(listaFil);
            }
        }

        //insertar
        public async Task<IActionResult> GuardarClientes()
        {
          
            ViewBag.paises = new SelectList(_pais.listado(), "idpais", "nombrepais");
            return View(await Task.Run(() => new Cliente()));
        }

        [HttpPost]
        public async Task<IActionResult> GuardarClientes(Cliente reg)
        {
            if (!ModelState.IsValid)
            {        
                ViewBag.paises = new SelectList(_pais.listado(), "idpais", "nombrepais", reg.idPais);
                return View(await Task.Run(() => reg));
            }

            ViewBag.mensaje = _clientes.Agregar(reg);
            ViewBag.paises = new SelectList(_pais.listado(), "idpais", "nombrepais", reg.idPais);
            return View(await Task.Run(() => reg));
        }

        //editar
        public async Task<IActionResult> EditarClientes(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {

                return RedirectToAction("ListarClientes", "Cliente");
            }
            
            Cliente reg = _clientes.Buscar(id);
            
            ViewBag.paises = new SelectList(_pais.listado(), "idpais", "nombrepais", reg.idPais);
            return View(await Task.Run(() => reg));
        }

        [HttpPost]
        public async Task<IActionResult> EditarClientes(Cliente reg)
        {
            if (!ModelState.IsValid)
            {
               
                ViewBag.paises = new SelectList(_pais.listado(), "idpais", "nombrepais", reg.idPais);
                return View(await Task.Run(() => reg));
            }  

            ViewBag.mensaje = _clientes.Actualizar(reg);
            ViewBag.clientes = _clientes.listado();
            ViewBag.paises = new SelectList(_pais.listado(), "idpais", "nombrepais", reg.idPais);
            return View(await Task.Run(() => reg));
        }

        //eliminar

        [HttpGet]
        public async Task<IActionResult> ClientesEliminar(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("ListarClientes", "Cliente");
            }

            Cliente reg = _clientes.Buscar(id);
            return View(await Task.Run(() => reg));
        }

        // POST: Realizar eliminación
        [HttpPost]
        [ActionName("ClientesEliminar")]
        public async Task<IActionResult> ClientesEliminarConfirmar(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("ListarClientes", "Cliente");
            }

            string mensaje = _clientes.Eliminar(id);
            ViewBag.mensaje = mensaje;
            return RedirectToAction("ListarClientes");
        }


    }
}
