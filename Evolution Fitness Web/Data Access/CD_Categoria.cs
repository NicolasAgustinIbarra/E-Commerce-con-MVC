using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
using CapaEntidad;

namespace Data_Access
{
    public class CD_Categoria
    {
        public List<Categoria> listar()
        {
            List<Categoria> listaCategoria = new List<Categoria>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select IdCategoria, Descripcion, Activo from CATEGORIA";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaCategoria.Add(new Categoria()
                            {
                                IdCategoria = Convert.ToInt32(reader["IdCategoria"]),
                                Activo = Convert.ToBoolean(reader["Activo"]),
                                Descripcion = reader["Descripcion"].ToString()
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

            return listaCategoria;

        }

        public int Registrar(Categoria obj, out string mensaje)
        {
            int idAutogenerado = 0;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarCategoria", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Configurar los parámetros de entrada
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);

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


        public bool Editar(Categoria obj, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarCategoria", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.IdCategoria);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);


                    // Configura los parámetros de salida
                    SqlParameter resultadoParam = cmd.Parameters.Add("@Resultado", SqlDbType.Bit);
                    resultadoParam.Direction = ParameterDirection.Output;

                    SqlParameter mensajeParam = cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500);
                    mensajeParam.Direction = ParameterDirection.Output;

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(resultadoParam.Value);
                    mensaje = mensajeParam.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }
            return resultado;
        }

        public bool Eliminar(int Id, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            using (SqlConnection conexion = new SqlConnection(Conexion.cn))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarCategoria", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("IdCategoria", Id);

                    // Configura los parámetros de salida
                    SqlParameter resultadoParam = cmd.Parameters.Add("@Resultado", SqlDbType.Bit);
                    resultadoParam.Direction = ParameterDirection.Output;

                    SqlParameter mensajeParam = cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500);
                    mensajeParam.Direction = ParameterDirection.Output;

                    conexion.Open();

                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(resultadoParam.Value);
                    mensaje = mensajeParam.Value.ToString();

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
