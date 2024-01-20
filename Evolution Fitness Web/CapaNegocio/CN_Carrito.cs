using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Data_Access;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Carrito
    {
        private CD_Carrito objCapaDatos = new CD_Carrito();

        public bool ExisteCarrito(int idcliente, int idproducto)
        {
            return objCapaDatos.ExisteCarrito(idcliente, idproducto);
        }
        public bool OpercaionCarrito(int idcliente, int idproducto, bool sumar, out string Mensaje)
        {
            return objCapaDatos.OperacionCarrito(idcliente, idproducto, sumar, out Mensaje);
        }
        public int CantidadEnCarrito(int idcliente)
        {
            return objCapaDatos.CantidadEnCarrito(idcliente);
        }

        public List<Carrito> listarProductos(int idcliente)
        {
            return objCapaDatos.listarProductos(idcliente);
        }
        public bool EliminarCarrito(int idcliente, int idproducto)
        {
            return objCapaDatos.EliminarCarrito(idcliente, idproducto);
        }
    }
}
