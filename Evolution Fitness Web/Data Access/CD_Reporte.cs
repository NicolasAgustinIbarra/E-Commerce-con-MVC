using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Data.SqlClient;
using System.Data;
using CapaEntidad;
using System.Globalization;

namespace Data_Access
{
    public class CD_Reporte
    {
        public List<Reporte> Ventas(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> lista = new List<Reporte>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select IdUsuario, Nombre, Apellidos, Correo,Pass, Restablecer,Activo from USUARIO";
                    SqlCommand cmd = new SqlCommand("sp_ReporteVentas", conexion);

                    cmd.Parameters.AddWithValue("fechainicio", fechainicio);
                    cmd.Parameters.AddWithValue("fechafin", fechafin);
                    cmd.Parameters.AddWithValue("idtransaccion", idtransaccion);

                    cmd.CommandType = CommandType.StoredProcedure;

                    conexion.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Reporte()
                            {
                                FechaVenta = Convert.ToDateTime(reader["FechaVenta"]),
                                Cliente = reader["Cliente"].ToString(),
                                Producto = reader["Producto"].ToString(),
                                Sabor = reader["Sabor"].ToString(),
                                IdTransaccion = reader["IdTransaccion"].ToString(),
                                Precio = Convert.ToDecimal(reader["Precio"], new CultureInfo("en-AR")),
                                Peso = Convert.ToInt32(reader["Peso"]),
                                Cantidad = Convert.ToInt32(reader["Cantidad"]),
                                Total = Convert.ToDecimal(reader["Total"])

                            }) ;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

              lista = new List<Reporte>();
            }
            finally
            {
                SqlConnection conexion = new SqlConnection(Conexion.cn);
                conexion.Close();
            }

            return lista;

        }
        public Dashboard VerDashboard()
        {

            Dashboard objeto = new Dashboard();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {

                    SqlCommand cmd = new SqlCommand("sp_ReporteDashboard", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    conexion.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            objeto = new Dashboard()
                            {
                                TotalCliente = Convert.ToInt32(reader["TotalCliente"]),
                                TotalVenta = Convert.ToInt32(reader["TotalVenta"]),
                                TotalProducto = Convert.ToInt32(reader["TotalProducto"]),

                            };
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                objeto = new Dashboard();
            }
            finally
            {
                SqlConnection conexion = new SqlConnection(Conexion.cn);
                conexion.Close();
            }

            return objeto;


        }


    }
}
