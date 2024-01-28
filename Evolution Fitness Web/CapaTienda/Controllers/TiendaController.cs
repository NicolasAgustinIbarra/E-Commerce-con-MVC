using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;

namespace CapaTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DetalleProducto(int idprodcuto = 0)
        {
            Producto oProducto = new Producto();
            bool conversion;

            oProducto = new CN_Producto().listar().Where(p => oProducto.IdProducto == idprodcuto).FirstOrDefault();

            if (oProducto != null)
            {
                oProducto.Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.RutaImagen, oProducto.NombreImagen), out conversion);
                oProducto.Extension = Path.GetExtension(oProducto.NombreImagen);
            }
            return View(oProducto);
        }

        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> lista = new List<Categoria>();

            lista = new CN_Categoria().listar();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult ListarMarcaPorCategoria(int? idcategoria)
        {
            try
            {
                List<Marca> lista = new List<Marca>();

                if (idcategoria == null)
                {
                    // Si idcategoria es null, obtén todas las marcas sin filtrar por categoría
                    lista = new CN_Marca().listar();
                }
                else
                {
                    // Si idcategoria tiene un valor, filtra por esa categoría
                    lista = new CN_Marca().listarmarcaporcategoria(idcategoria.Value);
                }

                return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Registra el error en algún lugar (consola, registro, etc.)
                Console.WriteLine($"Error: {ex.Message}");
                throw; // Re-lanza la excepción para que se informe al cliente
            }
        }


        [HttpPost]
        public JsonResult ListarProducto(int idmarca, int idcategoria)
        {
            List<Producto> lista = new List<Producto>();
            bool conversion;

            lista = new CN_Producto().listar().Select(x => new Producto()
            {
                IdProducto = x.IdProducto,
                Nombre = x.Nombre,
                Descripcion = x.Descripcion,
                ObjMarca = x.ObjMarca,
                ObjCategoria = x.ObjCategoria,
                Precio = x.Precio,
                Stock = x.Stock,
                Peso = x.Peso,
                Sabor = x.Sabor,
                RutaImagen = x.RutaImagen,
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(x.RutaImagen, x.NombreImagen), out conversion),
                Extension = Path.GetExtension(x.NombreImagen),
                Activo = x.Activo,

            }).Where(x => x.ObjCategoria.IdCategoria == (idcategoria == 0 ? x.ObjCategoria.IdCategoria : idcategoria) &&
            x.ObjMarca.IdMarca == (idmarca == 0 ? x.ObjMarca.IdMarca : idmarca) && x.Stock > 0 && x.Activo == true
            ).ToList();

            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult;


        }
        [HttpPost]
        public JsonResult ListarTodosProducto()
        {
            List<Producto> lista = new List<Producto>();
            bool conversion;

            lista = new CN_Producto().listarTodos().Select(x => new Producto
            {
                IdProducto = x.IdProducto,
                Nombre = x.Nombre,
                Descripcion = x.Descripcion,
                ObjMarca = x.ObjMarca,
                ObjCategoria = x.ObjCategoria,
                Precio = x.Precio,
                Stock = x.Stock,
                Peso = x.Peso,
                Sabor = x.Sabor,
                RutaImagen = x.RutaImagen,
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(x.RutaImagen, x.NombreImagen), out conversion),
                Extension = Path.GetExtension(x.NombreImagen),
                Activo = x.Activo,

            }).ToList();
            // Puedes agregar la lógica para transformar datos aquí si es necesario

            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult;
        }

        [HttpPost]
        public JsonResult AgregarCarrito(int idproducto)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            bool existe = new CN_Carrito().ExisteCarrito(idcliente, idproducto);

            bool respuesta = false;
            string mensaje = string.Empty;

            if (existe)
            {
                mensaje = "El producto ya existe en el carrito";
            }
            else
            {
                respuesta = new CN_Carrito().OpercaionCarrito(idcliente, idproducto, true, out mensaje);
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CantidadEnCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            int cantidad = new CN_Carrito().CantidadEnCarrito(idcliente);
            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProductosCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            List<Carrito> lista = new List<Carrito>();
            bool conversion;

            lista = new CN_Carrito().listarProductos(idcliente).Select(oc => new Carrito()
            {
                objProducto = new Producto()
                {
                    IdProducto = oc.objProducto.IdProducto,
                    Nombre = oc.objProducto.Nombre,
                    ObjMarca = oc.objProducto.ObjMarca,
                    Precio = oc.objProducto.Precio,
                    RutaImagen = oc.objProducto.RutaImagen,
                    Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oc.objProducto.RutaImagen, oc.objProducto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(oc.objProducto.NombreImagen)
                },
                Cantidad = oc.Cantidad,
                IdProducto = oc.IdProducto,
                IdCarrito = oc.IdCarrito,
                IdCliente = oc.IdCliente,


            }).ToList();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult OperacionCarrito(int idproducto, bool sumar)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;


            bool respuesta = false;
            string mensaje = string.Empty;



            respuesta = new CN_Carrito().OpercaionCarrito(idcliente, idproducto, true, out mensaje);


            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCarrito(int idproducto)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Carrito().EliminarCarrito(idcliente, idproducto);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Carrito()
        {
            return View();
        }


        [HttpPost]
        public async Task<JsonResult> ProcesarPago(List<Carrito> oListaCarrito, Venta oVenta)
        {
            decimal total = 0;

            DataTable detalle_venta = new DataTable();

            detalle_venta.Locale = new CultureInfo("es-AR");
            detalle_venta.Columns.Add("IdProducto", typeof(string));
            detalle_venta.Columns.Add("Cantidad", typeof(int));
            detalle_venta.Columns.Add("Total", typeof(decimal));

            
                foreach (Carrito oCarrito in oListaCarrito)
                {
                    // Verificar si oCarrito.objProducto es null
                    if (oCarrito.objProducto != null && oCarrito.objProducto.Precio != null)
                    {
                        decimal subTotal = Convert.ToDecimal(oCarrito.Cantidad.ToString()) * oCarrito.objProducto.Precio;

                        total += subTotal;

                        detalle_venta.Rows.Add(new object[]
                        {
                            oCarrito.objProducto.IdProducto,
                            oCarrito.Cantidad,
                            subTotal
                        });
                    }
                    else
                    {
                        // Manejo de la situación donde oCarrito.objProducto es null
                        // Puedes registrar un mensaje de error, ignorar este carrito, etc.
                        Console.WriteLine("Advertencia: oCarrito.objProducto es null");
                    }
                }


            

            oVenta.MontoTotal = total;
            oVenta.IdCliente.IdCliente = ((Cliente)Session["Cliente"]).IdCliente;

            TempData["Venta"] = oVenta;
            TempData["DetalleVenta"] = detalle_venta;

            return Json(new { status = true, Link = "/Tienda/PagoEfectuado?idTransaccion=code0001&status=true" }, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> PagoEfectuado()
        {
            string idtransaccion = Request.QueryString["idTransaccion"];
            bool status = Convert.ToBoolean( Request.QueryString["status"]);

            ViewData["status"] = status;

            if (status)
            {
                Venta oVenta = (Venta)TempData["Venta"];
                DataTable detalle_venta = (DataTable)TempData["DetalleVenta"];

                string mensaje = string.Empty;

                bool respuesta = new CN_Venta().Registrar(oVenta, detalle_venta, out mensaje);

                ViewData["idTransaccion"] = idtransaccion;
            }

            return View();
        }
    }

}