using CapaEntidad;
using Data_Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Producto
    {
        private CD_Producto objCapaDatos = new CD_Producto();

        public List<Producto> listar()
        {
            return objCapaDatos.listar();
        }
        public List<Producto> listarTodos()
        {
            return objCapaDatos.ListarTodosProductosDetallado();
        }
        public int Registrar(Producto obj, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                mensaje = "El nombre del producto no puede ser vacío";
            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                mensaje = "La descripción del producto no puede ser vacío";
            }

            else if (obj.ObjMarca.IdMarca == 0)
            {
                mensaje = "Debes seleccionar una marca";
            }

            else if (obj.ObjCategoria.IdCategoria == 0)
            {
                mensaje = "Debes seleccionar una categoría";
            }

            else if (obj.Precio == 0)
            {
                mensaje = "Debes ingresar un precio";
            }

            else if (obj.Peso == 0)
            {
                mensaje = "Debes ingresar el peso del producto";
            }
            
            else if (string.IsNullOrEmpty(obj.Sabor) || string.IsNullOrWhiteSpace(obj.Sabor))
            {
                mensaje = "Debes ingresar el sabor del producto";
            }
            else if (obj.Stock == 0)
            {
                mensaje = "Debes ingresar el stock del producto";
            }
            else if (obj.FechaVencimiento == null || obj.FechaVencimiento == DateTime.MinValue)
            {
                mensaje = "Debes ingresar la fecha de vencimiento del producto";
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

        public bool GuardarDatosImagen(Producto obj, out string mensaje)
        {
            return objCapaDatos.GuardarDatosImagen(obj, out mensaje);
        }

        public bool Editar(Producto obj, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                mensaje = "La descripción de la Producto no puede ser vacío";
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

