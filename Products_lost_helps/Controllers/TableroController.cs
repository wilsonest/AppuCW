using Microsoft.AspNetCore.Mvc;

namespace Products_lost_helps.Controllers
{
    public class TableroController : Controller
    {
        public IActionResult Tablero()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GuardarImagen(string imagenBase64)
        {
            var data = imagenBase64.Split(',')[1];
            var bytes = Convert.FromBase64String(data);

            var filePath = Path.Combine("wwwroot", "imagenes", "imagen_compuesta.png");
            await System.IO.File.WriteAllBytesAsync(filePath, bytes);

            return Json(new { success = true, path = $"/imagenes/imagen_compuesta.png" });
        }

    }
}
