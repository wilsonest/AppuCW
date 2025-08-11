using Microsoft.Extensions.Configuration;
using Products_lost_helps.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace Products_lost_helps.Logica
{
    public class Lo_Images : Images
    {

        private readonly IConfiguration _configuration;

        public Lo_Images(IConfiguration configuration)
        {
            _configuration = configuration;
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
    }
}
