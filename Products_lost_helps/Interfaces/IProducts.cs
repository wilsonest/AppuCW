using Products_lost_helps.Models;

namespace Products_lost_helps.Interfaces
{
    public interface IProducts
    {
        public List<Products> GetAllProducts();

        public Products CrearProducto(Products prod, int a);

        public Products GetProductos(int a);
    }
}
