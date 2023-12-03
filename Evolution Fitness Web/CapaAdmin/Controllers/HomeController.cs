using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;




using CapaNegocio;
using System.Collections;
using System.Web.Services.Description;
using System.Data;
using ClosedXML.Excel;
using System.IO;

namespace CapaAdmin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Usuario()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListaUsuarios()
        {
            List<Usuario> oLista = new List<Usuario>();
            oLista = new CN_Usuarios().listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarUsuario(Usuario objeto) 
        {
            object resultado;
            string mensaje = string.Empty;

            if(objeto.IdUsuario == 0)
            {
                resultado = new CN_Usuarios().Registrar(objeto, out mensaje);

            }
            else
            {
                resultado = new CN_Usuarios().Editar(objeto, out mensaje);
            }
            
            
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Usuarios().Eliminar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public JsonResult ListaReporte(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> oLista = new List<Reporte>();


            oLista = new CN_Reporte().Ventas(fechainicio,fechafin,idtransaccion);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult VistaDashboard()
        {
            Dashboard objeto = new CN_Reporte().VerDashboard();
            return Json(new { resultado = objeto}, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public FileResult ExportarVenta(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> oLista = new List<Reporte>();
            oLista = new CN_Reporte().Ventas(fechainicio, fechafin, idtransaccion);

            DataTable dataTable = new DataTable();
            dataTable.Locale = new System.Globalization.CultureInfo("en-AR");

            dataTable.Columns.Add("FechaVenta", typeof(DateTime));
            dataTable.Columns.Add("Cliente", typeof(string));
            dataTable.Columns.Add("Producto", typeof(string));
            dataTable.Columns.Add("Sabor", typeof(string));
            dataTable.Columns.Add("Cantidad", typeof(int));
            dataTable.Columns.Add("Peso", typeof(int));
            dataTable.Columns.Add("Precio", typeof(decimal));
            dataTable.Columns.Add("Total", typeof(decimal));
            dataTable.Columns.Add("IdTransaccion", typeof(string));

            foreach (Reporte rp in oLista)
            {
                dataTable.Rows.Add(new object[]
                {
                    rp.FechaVenta, // Asegúrate de que rp.FechaVenta sea de tipo DateTime
                    rp.Cliente,
                    rp.Producto,
                    rp.Sabor,
                    rp.Cantidad,
                    rp.Peso,
                    rp.Precio,
                    rp.Total,
                    rp.IdTransaccion
                });
            }

            dataTable.TableName = "Datos";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte Venta" + DateTime.Now.ToString() + ".xlsx");
                }
            }
        }

    }
}
