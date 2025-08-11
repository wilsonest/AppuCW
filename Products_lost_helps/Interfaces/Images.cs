using System.Data;

namespace Products_lost_helps.Interfaces
{
    public interface Images
    {
        public DataTable GetImagenes();
        public Task SubirImagen(int idProducto, List<IFormFile> archivos);
    }
}
