using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
using CapaEntidad;
using System.Security.Claims;

namespace Data_Access
{
    public class CD_Usuarios
    {
        public List<Usuario> listar()
        {
            List<Usuario> listaUsuarios = new List<Usuario>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select IdUsuario, Nombre, Apellidos, Correo,Pass, Restablecer,Activo from USUARIO";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaUsuarios.Add(new Usuario()
                            {
                                IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellidos"].ToString(),
                                Correo = reader["Correo"].ToString(),
                                Pass = reader["Pass"].ToString(),
                                Restablecer = Convert.ToBoolean(reader["Restablecer"]),
                                Activo = Convert.ToBoolean(reader["Activo"]),
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

            return listaUsuarios;

        }

        public int Registrar(Usuario obj, out string mensaje)
        {
            int idAutogenerado = 0;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Configurar los parámetros de entrada
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellido);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
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



        public bool Editar(Usuario obj, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarUsuario", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("IdUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellido);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);

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
                    SqlCommand cmd = new SqlCommand("delete top(1) from usuario where IdUsuario = @Id", conexion);
                    cmd.Parameters.AddWithValue("@Id", Id);
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

        public bool CambiarClave(int Idusuario, string nuevaclave, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            using (SqlConnection conexion = new SqlConnection(Conexion.cn))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("update USUARIO set Pass = @nuevaclave, Restablecer = 0 where Idusuario = @Idusuario ", conexion);
                    cmd.Parameters.AddWithValue("@Idusuario", Idusuario);
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
        public bool ReestablecerClave(int Idusuario, string clave, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            using (SqlConnection conexion = new SqlConnection(Conexion.cn))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("update USUARIO set Pass = @clave, Restablecer = 1 where Idusuario = @id ", conexion);
                    cmd.Parameters.AddWithValue("@Id", Idusuario);
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
