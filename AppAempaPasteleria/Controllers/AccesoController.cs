using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using AppAempaPasteleria.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace ProyectoFinal.Controllers
{
    public class AccesoController : Controller
    {
        string cadena = @"server=DESKTOP-HHJH2A8\SQLEXPRESS; database=Aempap2023; Trusted_connection=true;" +
        "MultipleActiveResultSets=true; TrustServerCertificate=false; Encrypt=false";

        IEnumerable<Usuario> usuarios()
        {
            List<Usuario> temporal = new List<Usuario>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_Login", cn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Usuario
                    {
                        Usuarios = dr.GetString(0),
                        Clave = dr.GetString(1),
                        Nombre= dr.GetString(2)
                    });
                }
                dr.Close();
            }
            return temporal;
        }
        public async Task< IActionResult> Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Usuario reg)
        {
            Usuario item=usuarios().FirstOrDefault( u => u.Usuarios == reg.Usuarios && u.Clave==reg.Clave);
            if(item==null)
            {
				ViewBag.mensaje = "Usuario no válido o no existente";
                return View();
            }
            else
            {
				var claim = new List<Claim>
				{
                    new Claim(ClaimTypes.Name,item.Nombre),
					new Claim("Usuarios",item.Usuarios),
                };

                var userIdentity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(userIdentity));
                

                return RedirectToAction("Panel", "Home");
            }
        }
        public async Task<IActionResult> Salir()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }
    }
}
