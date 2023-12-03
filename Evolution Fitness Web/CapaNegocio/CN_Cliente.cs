using CapaEntidad;
using Data_Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Cliente
    {

        private CD_Cliente objCapaDatos = new CD_Cliente();

        public List<Cliente> listar()
        {
            return objCapaDatos.listar();
        }

        public int Registrar(Cliente obj, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                mensaje = "El nombre del Cliente no puede ser vacío";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                mensaje = "El apellido del Cliente no puede ser vacío";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                mensaje = "El correo del Cliente no puede ser vacío";
            }

            if (string.IsNullOrEmpty(mensaje))
            {
                obj.Pass = CN_Recursos.ConvertirSha256(obj.Pass);
                return objCapaDatos.Registrar(obj, out mensaje);
                            
            }
            else
            {
                return 0;
            }
        }

       
        public bool CambiarClave(int IdCliente, string nuevaclave, out string mensaje)
        {

            return objCapaDatos.CambiarClave(IdCliente, nuevaclave, out mensaje);

        }
        public bool ReestablecerClave(int IdCliente, string correo, out string mensaje)
        {
            mensaje = string.Empty;
            string nuevaclave = CN_Recursos.GenerarClave();

            bool resultado = objCapaDatos.ReestablecerClave(IdCliente, CN_Recursos.ConvertirSha256(nuevaclave), out mensaje);

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

