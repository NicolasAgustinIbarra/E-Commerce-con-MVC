using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access
{
    public class CD_Cliente
    {
        public List<Cliente> listar()
        {
            List<Cliente> listaClientes = new List<Cliente>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select IdCliente, Nombres, Apellidos, Correo,Pass, Restablecer from Cliente";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaClientes.Add(new Cliente()
                            {
                                IdCliente = Convert.ToInt32(reader["IdCliente"]),
                                Nombres = reader["Nombres"].ToString(),
                                Apellidos = reader["Apellidos"].ToString(),
                                Correo = reader["Correo"].ToString(),
                                Pass = reader["Pass"].ToString(),
                                Restablecer = Convert.ToBoolean(reader["Restablecer"]),
                                
                            });
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                SqlConnection conexion = new SqlConnection(Conexion.cn);
                conexion.Close();
            }

            return listaClientes;

        }

        public int Registrar(Cliente obj, out string mensaje)
        {
            int idAutogenerado = 0;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarCliente", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Configurar los parámetros de entrada
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                 
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Pass", obj.Pass);

                    // Configurar los parámetros de salida
                    SqlParameter resultadoParam = cmd.Parameters.Add("@Resultado", SqlDbType.Int);
                    resultadoParam.Direction = ParameterDirection.Output;

                    SqlParameter mensajeParam = cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500);
                    mensajeParam.Direction = ParameterDirection.Output;

                    conexion.Open();

                    cmd.ExecuteNonQuery();

                    // Obtener los valores de salida después de ejecutar el comando
                    idAutogenerado = Convert.ToInt32(resultadoParam.Value);
                    mensaje = mensajeParam.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idAutogenerado = 0;
                mensaje = ex.Message;
            }
            return idAutogenerado;
        }

        public bool CambiarClave(int Idcliente, string nuevaclave, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            using (SqlConnection conexion = new SqlConnection(Conexion.cn))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("update CLIENTE set Pass = @nuevaclave, Restablecer = 0 where Idcliente = @Idusuario ", conexion);
                    cmd.Parameters.AddWithValue("@Idusuario", Idcliente);
                    cmd.Parameters.AddWithValue("@nuevaclave", nuevaclave);
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();

                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;

                }
                catch (Exception ex)
                {

                    resultado = false;
                    mensaje = ex.Message;
                }
                return resultado;


            }



        }

        public bool ReestablecerClave(int Idcliente, string clave, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            using (SqlConnection conexion = new SqlConnection(Conexion.cn))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("update CLIENTE set Pass = @clave, Restablecer = 1 where Idcliente = @id ", conexion);
                    cmd.Parameters.AddWithValue("@Id", Idcliente);
                    cmd.Parameters.AddWithValue("@Clave", clave);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();

                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;

                }
                catch (Exception ex)
                {

                    resultado = false;
                    mensaje = ex.Message;
                }
                return resultado;


            }
        }





    }
}
