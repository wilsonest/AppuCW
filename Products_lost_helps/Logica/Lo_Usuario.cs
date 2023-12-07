using Products_lost_helps.Models;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XSystem.Security.Cryptography;

namespace Products_lost_helps.Logica
{
    public class Lo_Usuario
    {

        private readonly IConfiguration _configuration;

        public Lo_Usuario(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public List<Products> GetAllProducts()
        {
            string cadena = _configuration.GetConnectionString("DefaultConnection");
            //var cone = cadena;

            List<Products> listaProductos = new List<Products>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                //SqlCommand cmd = new SqlCommand("select * from Productos", oconexion);
                //cmd.CommandType = CommandType.Text;
                //oconexion.Open();

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

        public DataTable GetImagenes()
        {
            string cadena = _configuration.GetConnectionString("DefaultConnection");

            DataTable dt = new DataTable();

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                string query = "select * from Fotos";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.CommandType = CommandType.Text;
                conexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    // Add columns to the DataTable
                    dt.Columns.Add("Id", typeof(int));
                    dt.Columns.Add("NombreImg", typeof(string));
                    dt.Columns.Add("Imagen", typeof(byte[]));
                    dt.Columns.Add("IdProducto", typeof(int));

                    while (dr.Read())
                    {
                        DataRow row = dt.NewRow();
                        row["Id"] = Convert.ToInt32(dr["Id"]);
                        row["NombreImg"] = dr["NombreImg"].ToString();
                        row["Imagen"] = (byte[])dr["Imagen"];
                        row["IdProducto"] = Convert.ToInt32(dr["IdProducto"]);
                        dt.Rows.Add(row);
                    }
                }
            }

            return dt;
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
                string query = "SELECT TOP 1 p.IdProducto, p.Nombre, p.Descripcion FROM Productos as p INNER JOIN Usuarios as s ON p.Id = s.Id WHERE s.Id = 1 ORDER BY p.IdProducto DESC";
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

        public async Task SubirImagen(int idProducto, List<IFormFile> archivos)
        {
            string cadena = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                await oconexion.OpenAsync();

                foreach (var archivo in archivos)
                {
                    if (archivo != null)
                    {
                        string nombre = Path.GetFileName(archivo.FileName);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await archivo.CopyToAsync(ms);
                            byte[] imagen = ms.ToArray();
                            SqlCommand cmd = new SqlCommand("insert into Fotos(NombreImg,Imagen,IdProducto) values(@nombreimg,@imagen,@idproducto)", oconexion);
                            cmd.Parameters.AddWithValue("@nombreimg", nombre);
                            cmd.Parameters.AddWithValue("@imagen", imagen);
                            cmd.Parameters.AddWithValue("@idproducto", idProducto);
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
        }


        public Users EncontrarUsuario(string correo, string clave)
        {

            string cadena = _configuration.GetConnectionString("DefaultConnection");

            var Clave = encrip(clave);
            //var Clave = EncryptStringAES(clave);

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                // query que se va a mandar
                string query = "select * from Usuarios where Correo = @pcorreo and Clave = @pclave";

                SqlCommand cmd = new SqlCommand(query, conexion);
                //llenamos los valores del query con los valores de entrada establecidos en el parametro
                cmd.Parameters.AddWithValue("@pcorreo", correo);
                cmd.Parameters.AddWithValue("@pclave", Clave);
                cmd.CommandType = CommandType.Text;
                conexion.Open();

                //los valores que traemos con el select
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    Users objeto = new Users();
                    while (dr.Read())
                    {
                        //var clavedes = DecryptStringFromBytes_Aes(Convert.FromBase64String(dr["Clave"].ToString()));  //prueba para zibor
                        //se llevan los atrubitos de la clase usuarios con lo que este en la bd
                        objeto = new Users()
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Nombre = dr["Nombre"].ToString(),
                            Apellido = dr["Apellido"].ToString(),
                            Correo = dr["Correo"].ToString(),
                            Clave = dr["Clave"].ToString(),
                            //Clave = clavedes,
                            IdRol = (Rol)dr["IdRol"],

                        };
                    }
                    return objeto;
                }
            }


        }


        public static string encrip(string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;

            //byte[] bytes = Encoding.Unicode.GetBytes(text);
            //SHA256Managed hashstring = new SHA256Managed();
            //byte[] hash = hashstring.ComputeHash(bytes);
            //return BitConverter.ToString(hash);

            //string finalKey = string.Empty;
            //byte[] encode = new byte[text.Length];
            //encode = Encoding.UTF8.GetBytes(text);
            //finalKey = Convert.ToBase64String(encode);
            //return finalKey;
        }


    }
}
