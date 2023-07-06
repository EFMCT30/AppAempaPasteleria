using AppAempaPasteleria.Modulos;
using AppAempaPasteleria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace AppAempaPasteleria.Controllers
{
	[Authorize]
	public class ProductoController : Controller
    {
        productosDAO _pro = new productosDAO();
        insumoDAO _insu = new insumoDAO();
        categoriaDAO _cate = new categoriaDAO();


        //listar
        public async Task<IActionResult> ListarProductos(string buscar="")
        {
            var listadoPro = _pro.listado();
            ViewBag.CANTIPRO = listadoPro.Count();
            if (string.IsNullOrEmpty(buscar))
            {
                return View(listadoPro);
            }
            else
            {
                var listaFil = listadoPro.Where(p => p.nomProd.StartsWith(buscar)).ToList();
                ViewBag.CANTIPRO = listaFil.Count();
                return View(listaFil);
            }
        }

        //insertar
       public async Task<IActionResult> GuardarProductos()
        {

            ViewBag.insumo = new SelectList(_insu.listado(), "idIn", "desIn");
            ViewBag.cate = new SelectList(_cate.listado(), "idCat", "nomCat");
            return View(await Task.Run(() => new Producto()));
        }

        [HttpPost]
        public ActionResult GuardarProductos(Producto objeto, IFormFile fotoProd)
        {
            try
            {
                if (fotoProd == null)
                {
                    ViewBag.MENSAJE = "Error: debe seleccionar una imagen";
                    return View(objeto);
                }

                if (ModelState.IsValid)
                {
                    // OBTENER EL NOMBRE DE LA IMAGEN PARA ALMACENAR EN "FOTOPER"
                    objeto.fotoProd = "~/Fotos/" + Path.GetFileName(fotoProd.FileName);

                    ViewBag.MENSAJE = _pro.Agregar(objeto);

                    // GUARDAR EL ARCHIVO IMAGEN EN LA CARPETA FOTOS
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Fotos", Path.GetFileName(fotoProd.FileName));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        fotoProd.CopyTo(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.MENSAJE = ex.Message;
            }
            ViewBag.insumo = new SelectList(_insu.listado(), "idIn", "desIn", objeto.idIn);
            ViewBag.cate = new SelectList(_cate.listado(), "idCat", "nomCat", objeto.idCate);
            return View(objeto);
        }

        //editar
        public async Task<IActionResult> EditarProductos(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {

                return RedirectToAction("ListarProductos", "Producto");
            }

            Producto reg = _pro.Buscar(id);

            ViewBag.insumo = new SelectList(_insu.listado(), "idIn", "desIn");
            ViewBag.cate = new SelectList(_cate.listado(), "idCat", "nomCat");
            return View(await Task.Run(() => reg));
        }

        [HttpPost]
        public async Task<IActionResult> EditarProductos(Producto reg)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.insumo = new SelectList(_insu.listado(), "idIn", "desIn", reg.idIn);
                ViewBag.cate = new SelectList(_cate.listado(), "idCat", "nomCat", reg.idCateD);
                return View(await Task.Run(() => reg));
            }

            ViewBag.mensaje = _pro.Actualizar(reg);
            ViewBag.productos = _pro.listado();
            ViewBag.insumo = new SelectList(_insu.listado(), "idIn", "desIn", reg.idIn);
            ViewBag.cate = new SelectList(_cate.listado(), "idCat", "nomCat", reg.idCateD);
            return View(await Task.Run(() => reg));
        }

        //editarImg
        public async Task<IActionResult> Editarimg(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {

                return RedirectToAction("ListarProductos", "Producto");
            }

            Producto reg = _pro.Buscar(id);

            ViewBag.insumo = new SelectList(_insu.listado(), "idIn", "desIn");
            ViewBag.cate = new SelectList(_cate.listado(), "idCat", "nomCat");
            return View(await Task.Run(() => reg));
        }

        [HttpPost]
        public ActionResult Editarimg(Producto objeto, IFormFile fotoProd)
        {
            try
            {
                if (fotoProd == null)
                {
                    ViewBag.MENSAJE = "Error: debe seleccionar una imagen";
                    return View(objeto);
                }

                if (ModelState.IsValid)
                {
                    // OBTENER EL NOMBRE DE LA IMAGEN PARA ALMACENAR EN "FOTOPER"
                    objeto.fotoProd = "~/Fotos/" + Path.GetFileName(fotoProd.FileName);

                    ViewBag.mensaje = _pro.Actualizar(objeto);

                    // GUARDAR EL ARCHIVO IMAGEN EN LA CARPETA FOTOS
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Fotos", Path.GetFileName(fotoProd.FileName));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        fotoProd.CopyTo(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.MENSAJE = ex.Message;
            }
            
            ViewBag.productos = _pro.listado();
            ViewBag.insumo = new SelectList(_insu.listado(), "idIn", "desIn", objeto.idIn);
            ViewBag.cate = new SelectList(_cate.listado(), "idCat", "nomCat", objeto.idCateD);
            return View(objeto);
        }

        //eliminar

        [HttpGet]
        public async Task<IActionResult> ProductoEliminar(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("ListarProductos", "Producto");
            }

            Producto reg = _pro.Buscar(id);
            return View(await Task.Run(() => reg));
        }

        // POST: Realizar eliminación
        [HttpPost]
        [ActionName("ProductoEliminar")]
        public async Task<IActionResult> ProductoEliminarConfirmar(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("ListarProductos", "Producto");
            }

            string mensaje = _pro.Eliminar(id);
            ViewBag.mensaje = mensaje;
            return RedirectToAction("ListarProductos");
        }

        
    }
}
