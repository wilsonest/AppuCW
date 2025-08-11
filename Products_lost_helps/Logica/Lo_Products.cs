using Microsoft.Extensions.Configuration;
using Products_lost_helps.Interfaces;
using Products_lost_helps.Models;
using System.Data;
using System.Data.SqlClient;

namespace Products_lost_helps.Logica
{
    public class Lo_Products: IProducts
    {

        private readonly IConfiguration _configuration;

        public Lo_Products(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Products> GetAllProducts()
        {
            string cadena = _configuration.GetConnectionString("DefaultConnection");

            List<Products> listaProductos = new List<Products>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {

                oconexion.Open(); // Aquí abres la conexión después de pasar la cadena de conexión

                SqlCommand cmd = new SqlCommand("select * from Productos", oconexion);
                cmd.CommandType = CommandType.Text;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Products archivoE = new Products();
                        archivoE.IdProducto = Convert.ToInt32(dr["IdProducto"]);
                        archivoE.Nombre = dr["Nombre"].ToString();
                        archivoE.Descripcion = dr["Descripcion"].ToString();
                        archivoE.Id = Convert.ToInt32(dr["Id"]);
                        listaProductos.Add(archivoE);
                    }
                }
            }
            return listaProductos;
        }

        public Products CrearProducto(Products prod, int a)
        {
            string cadena = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("insert into Productos(Nombre,Descripcion,Id) values(@nombre,@descripcion,@id)", oconexion);
                cmd.Parameters.AddWithValue("@nombre", prod.Nombre);
                cmd.Parameters.AddWithValue("@descripcion", prod.Descripcion);
                cmd.Parameters.AddWithValue("@id", a);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return prod;
        }

        public Products GetProductos(int a)
        {
            string cadena = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                //string query = "SELECT TOP 1 * FROM Productos INNER JOIN Usuarios ON Productos.Id = Usuarios.Id WHERE Usuarios.Id = @id ORDER BY Productos.IdProducto DESC";
                //string query = "SELECT TOP 1 p.IdProducto, p.Nombre, p.Descripcion, s.Nombre, s.Apellido, s.Correo FROM Productos as p INNER JOIN Usuarios as s ON p.Id = s.Id WHERE s.Id = 1 ORDER BY p.IdProducto DESC";
                //string query = "SELECT TOP 1 p.IdProducto, p.Nombre, p.Descripcion, p.Id FROM Productos as p INNER JOIN Usuarios as s ON p.Id = s.Id WHERE s.Id = 1 ORDER BY p.IdProducto DESC";
                string query = "SELECT TOP 1 * FROM Productos INNER JOIN Usuarios ON Productos.Id = Usuarios.Id WHERE Usuarios.Id = @id ORDER BY Productos.IdProducto DESC";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@id", a);
                cmd.CommandType = CommandType.Text;
                conexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    Products objeto = new Products();
                    while (dr.Read())
                    {
                        //se llevan los atrubitos de la clase usuarios con lo que este en la bd
                        objeto = new Products()
                        {
                            IdProducto = Convert.ToInt32(dr["IdProducto"]),
                            Nombre = dr["Nombre"].ToString(),
                            Descripcion = dr["Descripcion"].ToString(),
                            Id = Convert.ToInt32(dr["Id"]),
                        };
                    }
                    return objeto;
                }
            }


        }
    }
}
