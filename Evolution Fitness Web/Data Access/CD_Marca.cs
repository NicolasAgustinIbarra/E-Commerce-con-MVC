using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Data_Access
{
    public class CD_Marca
    {
        public List<Marca> listar()
        {
            List<Marca> listaMarca = new List<Marca>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select IdMarca, Descripcion, Activo from MARCA";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaMarca.Add(new Marca()
                            {
                                IdMarca = Convert.ToInt32(reader["IdMarca"]),
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

            return listaMarca;

        }

        public int Registrar(Marca obj, out string mensaje)
        {
            int idAutogenerado = 0;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarMarca", conexion);
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


        public bool Editar(Marca obj, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarMarca", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("IdMarca", obj.IdMarca);
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
                    SqlCommand cmd = new SqlCommand("sp_EliminarMarca", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("IdMarca", Id);

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

        public List<Marca> listarmarcaporcategoria(int idcategoria)
        {
            List<Marca> listaMarca = new List<Marca>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                   StringBuilder sb = new StringBuilder();
                    sb.AppendLine("select distinct m.IdMarca, m.Descripcion from PRODUCTO p");
                    sb.AppendLine("inner join CATEGORIA c on c.IdCategoria = p.IdCategoria");
                    sb.AppendLine("inner join MARCA m on m.IdMarca = p.IdMarca and m.Activo = 1");
                    sb.AppendLine("where c.IdCategoria = iif(@idcategoria = 0, c.IdCategoria, @idcategoria)");
            

                    SqlCommand cmd = new SqlCommand(sb.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@idcategoria", idcategoria);
                    cmd.CommandType = CommandType.Text;
                    

                    conexion.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaMarca.Add(new Marca()
                            {
                                IdMarca = Convert.ToInt32(reader["IdMarca"]),
                             
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

            return listaMarca;

        }
    }

}
