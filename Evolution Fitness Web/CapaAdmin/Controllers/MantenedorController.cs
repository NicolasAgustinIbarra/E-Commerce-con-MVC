using CapaEntidad;
using CapaNegocio;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaAdmin.Controllers
{
    [Authorize]
    public class MantenedorController : Controller
    {
        
        // GET: Mantenedor
        public ActionResult Marca()
        {
            return View();
        }
        public ActionResult Categoria()
        {
            return View();
        }
        public ActionResult Producto()
        {
            return View();
        }

        //++++++++++++++++++++++++++CATEGORIA+++++++++++++++++++++++++
        #region CATEGORIA
        [HttpGet]
        public JsonResult ListaCategorias()
        {
            List<Categoria> oLista = new List<Categoria>();
            oLista = new CN_Categoria().listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdCategoria == 0)
            {
                resultado = new CN_Categoria().Registrar(objeto, out mensaje);

            }
            else
            {
                resultado = new CN_Categoria().Editar(objeto, out mensaje);
            }


            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Categoria().Eliminar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        //++++++++++++++++++++++++++MARCA+++++++++++++++++++++++++
        #region MARCA
        [HttpGet]
        public JsonResult ListaMarcas()
        {
            List<Marca> oLista = new List<Marca>();
            oLista = new CN_Marca().listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GuardarMarca(Marca objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdMarca == 0)
            {
                resultado = new CN_Marca().Registrar(objeto, out mensaje);

            }
            else
            {
                resultado = new CN_Marca().Editar(objeto, out mensaje);
            }


            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Marca().Eliminar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        //++++++++++++++++++++++++++PRODUCTO+++++++++++++++++++++++++

        #region PRODUCTO

        [HttpGet]
        public JsonResult ListaProductos()
        {
            List<Producto> oLista = new List<Producto>();
            oLista = new CN_Producto().listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
        {
            object resultado;
            string mensaje = string.Empty;
            bool operacionExistosa = true;
            bool guardarImagenExito = true;

            Producto oProducto = new Producto();
            oProducto = JsonConvert.DeserializeObject<Producto>(objeto);

            decimal precio;
            if (decimal.TryParse(oProducto.PrecioTexto, System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("es-AR"), out precio))
            {
                oProducto.Precio = precio;
            }
            else
            {
                return Json(new { operacionExitosa = false, mensaje = "El formato del precio debe ser ##.##" }, JsonRequestBehavior.AllowGet);
            }




            if (oProducto.IdProducto == 0)
            {
                int idProductoGenerado = new CN_Producto().Registrar(oProducto, out mensaje);

                if (idProductoGenerado != 0)
                {
                    oProducto.IdProducto = idProductoGenerado;
                }
                else
                {
                    operacionExistosa = false;
                }
            }
            else
            {
                operacionExistosa = new CN_Producto().Editar(oProducto, out mensaje);
            }

            if (operacionExistosa)
            {
                if (archivoImagen != null)
                {
                    string ruta_guardar = ConfigurationManager.AppSettings["ServidorFoto"];
                    string extension = Path.GetExtension(archivoImagen.FileName);
                    string nombre_imagen = string.Concat(oProducto.IdProducto.ToString(), extension);

                    // Obtén la ruta física del directorio
                    string rutaDirectorio = Server.MapPath("~/fotos_carrito");

                    // Verifica si la carpeta existe, y si no, créala
                    if (!Directory.Exists(rutaDirectorio))
                    {
                        Directory.CreateDirectory(rutaDirectorio);
                    }

                    // Combina la ruta del directorio con el nombre del archivo
                    string rutaCompleta = Path.Combine(rutaDirectorio, nombre_imagen);

                    try
                    {
                        archivoImagen.SaveAs(rutaCompleta);
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        guardarImagenExito = false;
                    }

                    if (guardarImagenExito)
                    {

                        oProducto.RutaImagen = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["ServidorFoto"]));
                        oProducto.NombreImagen = nombre_imagen;
                        bool rspt = new CN_Producto().GuardarDatosImagen(oProducto, out mensaje);
                    }
                    else
                    {
                        mensaje = "Se guardó el producto, pero hubo problemas con la imagen";
                    }
                }
            }



            return Json(new { operacionExitosa = operacionExistosa, idGenerado = oProducto.IdProducto, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {

            bool conversion;

            Producto producto = new CN_Producto().listar().Where(p => p.IdProducto == id).FirstOrDefault();
            string base64 = CN_Recursos.ConvertirBase64(Path.Combine(producto.RutaImagen, producto.NombreImagen), out conversion);


            return Json(new
            {
                conversion = conversion,
                textobase64 = base64,
                extension = Path.GetExtension(producto.NombreImagen)

            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Producto().Eliminar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        #endregion

    }
}