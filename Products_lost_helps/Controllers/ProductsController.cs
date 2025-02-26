using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Products_lost_helps.Logica;
using Products_lost_helps.Models;
using System.Data;

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
                return RedirectToAction("Servicios", "Principal");
            }
            return RedirectToAction("Servicios", "Principal");
        }


        public ActionResult InfoProdutcs(int idProducto, string descripcion)
        {

            DataTable dt = _loUsuario.GetImagenes();
            List<string> imagenes = new List<string>();

            foreach (DataRow row in dt.Rows)
            {
                if ((int)row["IdProducto"] == idProducto)
                {
                    byte[] imagen = (byte[])row["Imagen"];
                    string base64String = Convert.ToBase64String(imagen);
                    string imgSrc = string.Format("data:image/jpeg;base64,{0}", base64String);
                    imagenes.Add(imgSrc);
                }
            }

            ViewBag.Descripcion = descripcion;
            ViewBag.IdProducto = idProducto;
            ViewBag.Imagenes = imagenes;

            return View();
        }
    }
}
