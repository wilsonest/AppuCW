namespace Products_lost_helps.Models
{
    public class Images
    {
        public int Id { get; set; }
        public string? NombreImg { get; set; }
        public byte[]? Imagen { get; set; }
        public int IdProducto { get; set; }
    }
}
