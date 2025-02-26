using Products_lost_helps.Models;
using System.Data;
using System.Data.SqlClient;

namespace Products_lost_helps.Logica
{
    public class Lo_Clients
    {
        private readonly IConfiguration _configuration;

        public Lo_Clients(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IEnumerable<Clients> GetAllClients()
        {
            string cadena = _configuration.GetConnectionString("DefaultConnection");
            //var cone = cadena;

            List<Clients> listaProductos = new List<Clients>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                oconexion.Open(); // Aquí abres la conexión después de pasar la cadena de conexión

                SqlCommand cmd = new SqlCommand("select * from Clients", oconexion);
                cmd.CommandType = CommandType.Text;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Clients archivoE = new Clients();
                        archivoE.Id = Convert.ToInt32(dr["Id"]);
                        archivoE.Nombre = dr["Nombre"].ToString();
                        archivoE.Mesa = Convert.ToInt32(dr["Mesa"]);
                        archivoE.Cuenta = Convert.ToDecimal(dr["Cuenta"]);

                        listaProductos.Add(archivoE);
                    }
                }
            }
            return listaProductos;
        }
    }
}
