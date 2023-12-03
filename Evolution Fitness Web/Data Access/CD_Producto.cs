using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace Data_Access
{
    public class CD_Producto
    {
        public List<Producto> listar()
        {
            List<Producto> listaProductos = new List<Producto>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    string query = @"SELECT p.IdProducto, p.Nombre, p.Descripcion,
                         m.IdMarca, m.Descripcion AS DesMarca, 
                         c.IdCategoria, c.Descripcion AS DesCategoria,
                         p.Peso, p.Activo, p.FechaVencimiento, p.Precio, p.Stock,
                         p.Sabor,p.RutaImagen,p.NombreImagen
                     FROM PRODUCTO p
                     INNER JOIN MARCA m ON m.IdMarca = p.IdMarca
                     INNER JOIN CATEGORIA c ON c.IdCategoria = p.IdCategoria";

                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaProductos.Add(new Producto()
                            {
                                IdProducto = Convert.ToInt32(reader["IdProducto"]),
                                Nombre = reader["Nombre"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                                ObjMarca = new Marca { IdMarca = Convert.ToInt32(reader["IdMarca"]), Descripcion = reader["DesMarca"].ToString() },
                                ObjCategoria = new Categoria { IdCategoria = Convert.ToInt32(reader["IdCategoria"]), Descripcion = reader["DesCategoria"].ToString() },
                                Peso = Convert.ToDecimal(reader["Peso"]),
                                Activo = Convert.ToBoolean(reader["Activo"]),
                                FechaVencimiento = Convert.ToDateTime(reader["FechaVencimiento"]),
                                Precio = Convert.ToDecimal(reader["Precio"],
                                new CultureInfo("es-AR")),
                                Stock = Convert.ToInt32(reader["Stock"]),
                                Sabor = reader["Sabor"].ToString(),
                                RutaImagen = reader["RutaImagen"].ToString(),
                                NombreImagen = reader["NombreImagen"].ToString(),

                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones aquí, por ejemplo, registrar el error o lanzar una excepción personalizada.
                throw ex;
            }
            // No necesitas abrir una nueva conexión aquí; esta se cierra automáticamente al salir del bloque "using".

            return listaProductos;
        }
        public int Registrar(Producto obj, out string mensaje)
        {
            int idAutogenerado = 0;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarProducto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Configurar los parámetros de entrada
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("IdMarca", obj.ObjMarca.IdMarca);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.ObjCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Peso", obj.Peso);
                    cmd.Parameters.AddWithValue("FechaVencimiento", obj.FechaVencimiento);
                    cmd.Parameters.AddWithValue("Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("Stock", obj.Stock);
                    cmd.Parameters.AddWithValue("Sabor", obj.Sabor);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.AddWithValue("RutaImagen", obj.RutaImagen ?? ""); // Asegúrate de que no sea nulo
                    cmd.Parameters.AddWithValue("NombreImagen", obj.NombreImagen ?? ""); // Asegúrate de que no sea nulo



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
        public bool Editar(Producto obj, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarProducto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("IdProducto", obj.IdProducto);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("IdMarca", obj.ObjMarca.IdMarca);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.ObjCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Peso", obj.Peso);
                    cmd.Parameters.AddWithValue("FechaVencimiento", obj.FechaVencimiento);
                    cmd.Parameters.AddWithValue("Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("Stock", obj.Stock);
                    cmd.Parameters.AddWithValue("Sabor", obj.Sabor);
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
                    SqlCommand cmd = new SqlCommand("sp_EliminarProducto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("IdProducto", Id);

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
        public bool GuardarDatosImagen(Producto obj, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {

                    string query = "update PRODUCTO set RutaImagen = @RutaImagen, NombreImagen = @NombreImagen where IdProducto = @IdProducto";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("IdProducto", obj.IdProducto);
                    cmd.Parameters.AddWithValue("RutaImagen", obj.RutaImagen);
                    cmd.Parameters.AddWithValue("NombreImagen", obj.NombreImagen);


                    conexion.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        resultado = false;
                    }
                    else
                    {
                        mensaje = "No se pudo actualizar la imagen";
                    }



                }
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

