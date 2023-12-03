using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Data_Access;
using CapaEntidad;
using System.Security.Claims;

namespace CapaNegocio
{
    public class CN_Usuarios
    {
        private CD_Usuarios objCapaDatos = new CD_Usuarios();
        
       public List<Usuario> listar()
        {
            return objCapaDatos.listar();
        }

        public int Registrar(Usuario obj, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                mensaje = "El nombre del usuario no puede ser vacío";
            }
            else if (string.IsNullOrEmpty(obj.Apellido) || string.IsNullOrWhiteSpace(obj.Apellido))
            {
                mensaje = "El apellido del usuario no puede ser vacío";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                mensaje = "El correo del usuario no puede ser vacío";
            }

            if (string.IsNullOrEmpty(mensaje))
            {
                                            
                                
                string clave = CN_Recursos.GenerarClave();
                string asunto = "Creación de cuenta";
                string mensajeCorreo = "<h3>Su cuenta ha sido creada exitosamente!</h3></br><p>Su contraseña para acceder es : !Clave¡</p>";
                mensajeCorreo = mensajeCorreo.Replace("!Clave¡", clave);

                bool respuesta = CN_Recursos.EnviarCorreo(obj.Correo, mensajeCorreo, asunto);

                if (respuesta)
                {
                    obj.Pass = CN_Recursos.ConvertirSha256(clave);
                    return objCapaDatos.Registrar(obj, out mensaje);
                }
                else
                {
                    mensaje = "No se puede enviar el mensaje";
                    return 0;
                }
                

            }
            else
            {
                return 0;
            }
        }




        public bool Editar(Usuario obj, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                mensaje = "El nombre del usuario no puede ser vacio";
            }
            else if ((string.IsNullOrEmpty(obj.Apellido) || string.IsNullOrWhiteSpace(obj.Apellido)))
            {
                mensaje = "El apellido del usuario no puede ser vacio";
            }
            else if ((string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo)))
            {
                mensaje = "El correo del usuario no puede ser vacio";
            }



            if (string.IsNullOrEmpty(mensaje))
            {
                return objCapaDatos.Editar(obj, out mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(int Id, out string mensaje)
        {

            return objCapaDatos.Eliminar(Id, out mensaje);

        }
        
        public bool CambiarClave(int Idusuario, string nuevaclave, out string mensaje)
        {

            return objCapaDatos.CambiarClave(Idusuario, nuevaclave, out mensaje);

        }
        public bool ReestablecerClave(int Idusuario, string correo, out string mensaje)
        {
            mensaje = string.Empty;
            string nuevaclave = CN_Recursos.GenerarClave();

            bool resultado = objCapaDatos.ReestablecerClave(Idusuario,CN_Recursos.ConvertirSha256(nuevaclave), out mensaje);

            if (resultado)
            {

                string asunto = "Reestablecer contraseña";
                string mensajeCorreo = "<h3>Su cuenta ha sido reestablecida exitosamente!</h3></br><p>Su contraseña para acceder es : !Clave¡</p>";
                mensajeCorreo = mensajeCorreo.Replace("!Clave¡", nuevaclave);

                bool respuesta = CN_Recursos.EnviarCorreo(correo, mensajeCorreo, asunto);

                if (respuesta)
                {

                    return true;
                }
                else
                {
                    mensaje = "No se pudo enviar el correo";
                    return false;
                }
            }
            else
            {
                mensaje = "No se pudo reestablecer la contraseña";
                return false;
            }

            
        }
    }
}
