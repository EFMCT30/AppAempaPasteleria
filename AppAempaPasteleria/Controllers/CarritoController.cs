using Microsoft.AspNetCore.Session;
using System.Data;
using AppAempaPasteleria.Models;
using AppAempaPasteleria.Modulos;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using ClosedXML.Excel;

namespace AppAempaPasteleria.Controllers
{
	[Authorize]
	public class CarritoController : Controller
    {
        productosDAO _pro = new productosDAO();
        clienteDAO _cli=new clienteDAO();
        categoriaDAO _cate = new categoriaDAO();
        string cadena = @"server=DESKTOP-HHJH2A8\SQLEXPRESS;Database=Aempap2023;Trusted_Connection=true;MultipleActiveResultSets=True;TrustServerCertificate=false;Encrypt=false;";


        //listarCarrito
        public async Task<IActionResult> Portal(string buscar = "")
        {
            if (HttpContext.Session.GetString("canasta") == null)
            {
                HttpContext.Session.SetString("canasta",
                  JsonConvert.SerializeObject(new List<Registro>()));
            }

            ViewBag.cate = new SelectList(_cate.listado(), "idCat", "nomCat");
            var listado = _pro.listado();
            var listaPro = await Task.Run(() => listado);
            ViewBag.CANTIPRO = listaPro.Count();
            if (string.IsNullOrEmpty(buscar))
            {
                return View(listaPro);
            }
            else
            {
                var listaFil = listaPro.Where(p => p.idCate.StartsWith(buscar)).ToList();
                ViewBag.CANTIPRO = listaFil.Count();
                return View(listaFil);
            }
        }

        //selecionar
        public async Task<IActionResult> Seleccionar(string? id = null)
        {
            if (id == null) return RedirectToAction("Portal");
            var buscar = _pro.Buscar(id);
            return View(await Task.Run(() => buscar));
        }

        [HttpPost]
        public async Task<IActionResult> Seleccionar(string codigo, int cantidad)
        {
            List<Registro> temporal = JsonConvert.DeserializeObject<List<Registro>>(HttpContext.Session.GetString("canasta"));
            Registro reg = temporal.FirstOrDefault(x => x.idProd == codigo);
            if (reg == null)
            {

                Producto it = _pro.Buscar(codigo);
                reg = new Registro()
                {
                    idProd = it.idProd,
                    nomProd = it.nomProd,
                    fotoProd = it.fotoProd,
                    idIn=it.idIn,
                    idCate= it.idCate,
                    descProd=it.descProd,
                    preProd=it.preProd,
                    stockProd=cantidad,

                };
                temporal.Add(reg);
            }
            else
            {
                reg.stockProd += cantidad;
                //si lo encontro, incremento la cantidad del producto seleccionado
                if (reg.stockProd != null) { ViewBag.mensaje = "Ya registrado"; }
            }
            var buscar = _pro.Buscar(codigo);
            ViewBag.mensaje = $"Se ha registrado el Producto {reg.nomProd} con {reg.stockProd} unidades";
            ViewBag.desactivarBtn = true;
            HttpContext.Session.SetString("canasta", JsonConvert.SerializeObject(temporal));
            return View(await Task.Run(() => buscar));
        }

        //canasta
        public async Task<IActionResult> Canasta()
        {

            if (HttpContext.Session.GetString("canasta") == null)
                return RedirectToAction("Portal");

            IEnumerable<Registro> temporal =
              JsonConvert.DeserializeObject<IEnumerable<Registro>>(HttpContext.Session.GetString("canasta"));

            return View(await Task.Run(() => temporal));
        }

        //actualizar
        public IActionResult Actualizar(string codigo, int cantidad)
        {

            List<Registro> temporal = JsonConvert.DeserializeObject<List<Registro>>(HttpContext.Session.GetString("canasta"));

            Registro reg = temporal.FirstOrDefault(p => p.idProd == codigo);
            if (reg != null)
            {
                reg.stockProd = cantidad;
            }

            HttpContext.Session.SetString("canasta", JsonConvert.SerializeObject(temporal));

            return RedirectToAction("Canasta");
        }

        //eliminar
        public IActionResult Eliminar(string id)
        {
            List<Registro> temporal =
              JsonConvert.DeserializeObject<List<Registro>>(HttpContext.Session.GetString("canasta"));

            temporal.RemoveAll(x => x.idProd == id);

            HttpContext.Session.SetString("canasta", JsonConvert.SerializeObject(temporal));

            return RedirectToAction("Canasta");
        }

        //compra
        public IActionResult Compra()
        {
            var clientes = _cli.listado().Select(c => new
            {
                codCli = c.codCli,
                nombreCompleto = $"{c.nomCli} {c.apeCli}"
            });

            ViewBag.clientes = new SelectList(clientes, "codCli", "nombreCompleto");
            //ViewBag.clientes = new SelectList(_cli.listado(), "codCli", "nomCli");

            return View();

        }

       
        [HttpPost]
        public IActionResult Compra(int idPed, string codcli, string metodoPed, string EntregaPed, string EstadoPed)
        {
            //deserializo el Session canasta
            List<Registro> temporal = JsonConvert.DeserializeObject<List<Registro>>(HttpContext.Session.GetString("canasta"));

            //si aun no tiene registros, redireccionar al Portal
            if (temporal.Count() == 0)
            {
                return RedirectToAction("Portal", new { buscar = "" });
            }

            string mensajes = "";
            SqlConnection cn = new SqlConnection(cadena);
            cn.Open();
            SqlTransaction t = cn.BeginTransaction(System.Data.IsolationLevel.Serializable);

            try
            {
                SqlCommand cmd = new SqlCommand("usp_AgregarPedido", cn, t); //conexion y transaccion
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idPed", idPed);
                cmd.Parameters.AddWithValue("@idCli", codcli);
                cmd.Parameters.AddWithValue("@metodoPed", metodoPed);
                cmd.Parameters.AddWithValue("@EntregaPed", EntregaPed);
                cmd.Parameters.AddWithValue("@EstadoPed", EstadoPed);
                cmd.ExecuteNonQuery(); //ejecutar

                foreach (var item in temporal) //recorrer los elementos del temporal para agregar pedidosdeta
                {
                    cmd = new SqlCommand("usp_AgregarDetaPedido", cn, t);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idPed", idPed);
                    cmd.Parameters.AddWithValue("@idProd", item.idProd);
                    cmd.Parameters.AddWithValue("@preUni", item.preProd);
                    cmd.Parameters.AddWithValue("@cantidad", item.stockProd);
                    cmd.ExecuteNonQuery();
                }

                foreach (var item in temporal) //recorrer los elementos del temporal para actualizar 
                {
                    cmd = new SqlCommand("usp_productosActualizar", cn, t);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idProd", item.idProd);
                    cmd.Parameters.AddWithValue("@cantidad", item.stockProd);
                    cmd.ExecuteNonQuery();
                }

                t.Commit(); //si todo está OK, actualizamos la bd
                ViewBag.SuccessMessage = "Se ha registrado el pedido con éxito.";
                HttpContext.Session.Remove("canasta");

                var clientes = _cli.listado().Select(c => new
                {
                    codCli = c.codCli,
                    nombreCompleto = $"{c.nomCli} {c.apeCli}"
                });

                ViewBag.clientes = new SelectList(clientes, "codCli", "nombreCompleto");
                return RedirectToAction("ListarPedido","Pedido");
            }
            catch (SqlException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                t.Rollback(); //en caso de error deshacer

                var clientes = _cli.listado().Select(c => new
                {
                    codCli = c.codCli,
                    nombreCompleto = $"{c.nomCli} {c.apeCli}"
                });

                ViewBag.clientes = new SelectList(clientes, "codCli", "nombreCompleto");
                return View(); // Mantener la vista actual en caso de error
            }
            finally
            {
                cn.Close();
            }
        }

        //
        public IActionResult ExportarExcel()
        {
            DataTable tabla_pedido = new DataTable();
            SqlConnection cn = new SqlConnection(cadena);
            //=========== PRIMERO - OBTENER EL DATA ADAPTER ===========
            using (cn)
            {
                cn.Open();
                using (var adapter = new SqlDataAdapter())
                {

                    adapter.SelectCommand = new SqlCommand("usp_ListarPedidoExcel", cn);
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                    adapter.Fill(tabla_pedido);
                }
            }


            //usar referencias
            //=========== SEGUNDO - INSTALAR ClosedXML ===========
            using (var libro = new XLWorkbook())
            {

                tabla_pedido.TableName = "Pedidos";
                var hoja = libro.Worksheets.Add(tabla_pedido);
                hoja.ColumnsUsed().AdjustToContents();

                using (var memoria = new MemoryStream())
                {

                    libro.SaveAs(memoria);

                    var nombreExcel = string.Concat("Reporte ", DateTime.Now.ToString(), ".xlsx");

                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }

        }

    }
}
