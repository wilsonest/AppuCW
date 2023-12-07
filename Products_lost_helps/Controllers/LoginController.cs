using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Products_lost_helps.Logica;
using Products_lost_helps.Models;
using System.Security.Claims;
using XAct;

namespace Products_lost_helps.Controllers
{
    public class LoginController : Controller
    {
        private readonly Lo_Usuario _loUsuario;

        public LoginController(Lo_Usuario loUsuario)
        {
            _loUsuario = loUsuario;
        }
        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult Index(string correo, string clave)
        //{
        //    //mandamos los parametros al modelo y metodo
        //    Users objeto = _loUsuario.EncontrarUsuario(correo, clave);

        //    //Session["Usuario"] = objeto.Nombre;
        //    if (objeto.Nombre != null)
        //    {
        //        FormsAuthentication.SetAuthCookie(objeto.ToString(), false);

        //        Session["UsuarioNombre"] = objeto.Nombre;
        //        Session["IdUsuario"] = objeto.Id;

        //        return RedirectToAction("Index", "Principal");
        //    }


        //    return View();
        //}



        [HttpPost]
        public async Task<IActionResult> LogIn(string correo, string clave)
        {

            Users objeto = _loUsuario.EncontrarUsuario(correo, clave);
            if (objeto == null)
            {
                ViewData["MENSAJE"] = "No tienes credenciales correctas";
                return View();
            }
            else
            {
                //DEBEMOS CREAR UNA IDENTIDAD (name y role)
                //Y UN PRINCIPAL
                //DICHA IDENTIDAD DEBEMOS COMBINARLA CON LA COOKIE DE 
                //AUTENTIFICACION
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                //TODO USUARIO PUEDE CONTENER UNA SERIE DE CARACTERISTICAS
                //LLAMADA CLAIMS.  DICHAS CARACTERISTICAS PODEMOS ALMACENARLAS
                //DENTRO DE USER PARA UTILIZARLAS A LO LARGO DE LA APP
                Claim claimUserName = new Claim(ClaimTypes.Name, objeto.Nombre);
                //Claim claimRole = new Claim(ClaimTypes.Role, objeto.IdRol);
                Claim claimIdUsuario = new Claim("IdUsuario", objeto.Id.ToString());
                Claim claimEmail = new Claim("EmailUsuario", objeto.Correo);
                

                identity.AddClaim(claimUserName);
                //identity.AddClaim(claimRole);
                identity.AddClaim(claimIdUsuario);
                identity.AddClaim(claimEmail);

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.Now.AddMinutes(45)
                });

                var aut = identity.IsAuthenticated;
                return RedirectToAction("Index", "Principal");
            }

        }


        public IActionResult ErrorAcceso()
        {
            ViewData["MENSAJE"] = "Error de acceso";
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
