using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Data_Access;
using CapaEntidad;
using System.Data;

namespace CapaNegocio
{
    public class CN_Venta
    {
        private CD_Venta objCapaDato = new CD_Venta();
        public bool Registrar(Venta obj, DataTable detalleVenta, out string mensaje)
        {
            return objCapaDato.Registrar(obj, detalleVenta, out mensaje);
        }
    }
}
