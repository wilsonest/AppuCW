using Microsoft.AspNetCore.Mvc;
using Products_lost_helps.Logica;
using Products_lost_helps.Models;
using System.Data;
using System.Linq;

namespace Products_lost_helps.Controllers
{
    public class PrincipalController : Controller
    {
        private readonly Lo_Usuario _loUsuario;

        public PrincipalController(Lo_Usuario loUsuario)
        {
            _loUsuario = loUsuario;
        }
        public IActionResult Index()
        {
            List<Products> productos = _loUsuario.GetAllProducts();
            return View(productos);
        }

        public IActionResult Servicios()
        {
            List<Products> productos = _loUsuario.GetAllProducts();
            return View(productos);
        }

        public ActionResult ver(int IdProducts)
        {
            //Lo_Usuario lo = new Lo_Usuario();
            DataTable dt = _loUsuario.GetImagenes();
            DataRow row = dt.AsEnumerable().FirstOrDefault(r => r.Field<int>("IdProducto") == IdProducts);
            if (row != null)
            {
                byte[] imgc = (byte[])row["Imagen"];
                return File(imgc, "application/octet-stream");
            }
            return RedirectToAction("Index", "Principal");
        }

        public ActionResult verfirst(int IdProducts)
        {
            //Lo_Usuario lo = new Lo_Usuario();
            DataTable dt = _loUsuario.GetImagenes();
            DataRow row = dt.AsEnumerable().FirstOrDefault(r => r.Field<int>("IdProducto") == IdProducts);
            if (row != null)
            {
                byte[] imgc = (byte[])row["Imagen"];
                return File(imgc, "application/octet-stream");
            }
            return RedirectToAction("Index", "Principal");
        }
    }
}
