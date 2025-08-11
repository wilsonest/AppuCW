using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Products_lost_helps.Interfaces;
using Products_lost_helps.Logica;
using Products_lost_helps.Models;
using System.Data;

namespace Products_lost_helps.Controllers
{
    public class ProductsController : Controller
    {
        private readonly Lo_Products _loProducts;
        private readonly Lo_Images _loImages;

        public ProductsController(Lo_Products loProducts, Lo_Images loImages)
        {
            _loProducts = loProducts;
            _loImages = loImages;
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
                Products objeto = _loProducts.CrearProducto(prod, a);
                Products producto = _loProducts.GetProductos(a);
                int b = producto.IdProducto;

                if (Archivos != null && Archivos.Count > 0)
                {
                    await _loImages.SubirImagen(b, Archivos);
                }
                ViewBag.Message = "Producto Creado";
                return RedirectToAction("Servicios", "Principal");
            }
            return RedirectToAction("Servicios", "Principal");
        }


        public ActionResult InfoProdutcs(int idProducto, string descripcion)
        {

            DataTable dt = _loImages.GetImagenes();
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
