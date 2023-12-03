using CapaEntidad;
using Data_Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Marca
    {
        private CD_Marca objCapaDatos = new CD_Marca();

        public List<Marca> listar()
        {
            return objCapaDatos.listar();
        }

        public int Registrar(Marca obj, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                mensaje = "La descripción de la marca no puede ser vacío";
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

        public bool Editar(Marca obj, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                mensaje = "La descripción de la marca no puede ser vacío";
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
        public List<Marca> listarmarcaporcategoria(int idcategoria)
        {
            return objCapaDatos.listarmarcaporcategoria(idcategoria);
        }
    }
}
