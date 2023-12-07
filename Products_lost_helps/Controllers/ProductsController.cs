using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Products_lost_helps.Logica;
using Products_lost_helps.Models;

namespace Products_lost_helps.Controllers
{
    public class ProductsController : Controller
    {
        private readonly Lo_Usuario _loUsuario;

        public ProductsController(Lo_Usuario loUsuario)
        {
            _loUsuario = loUsuario;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CrearProduct(Products prod, List<IFormFile> Archivos)
        {
            if (prod.Nombre == null || prod.Descripcion == null)
            {
                return View("Error", new { message = "Falla al registrarte" });
            }
            else if (prod.Nombre != null)
            {

                int a = 1;
                Products objeto = _loUsuario.CrearProducto(prod, a);
                Products producto = _loUsuario.GetProductos(a);
                int b = producto.IdProducto;

                if (Archivos != null && Archivos.Count > 0)
                {
                    await _loUsuario.SubirImagen(b, Archivos);
                }
                ViewBag.Message = "Producto Creado";
                return RedirectToAction("Index", "Principal");
            }
            return RedirectToAction("Index", "Principal");
        }
    }
}
