using Data_Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Data_Access;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Categoria
    {
        private CD_Categoria objCapaDatos = new CD_Categoria();

        public List<Categoria> listar()
        {
            return objCapaDatos.listar();
        }

        public int Registrar(Categoria obj, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                mensaje = "La descripción de la categoría no puede ser vacío";
            }
          

            if (string.IsNullOrEmpty(mensaje))
            {

                                                               
                    return objCapaDatos.Registrar(obj, out mensaje);
                

            }
            else
            {
                return 0;
            }
        }

        public bool Editar(Categoria obj, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                mensaje = "La descripción de la categoría no puede ser vacío";
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
    }
}
