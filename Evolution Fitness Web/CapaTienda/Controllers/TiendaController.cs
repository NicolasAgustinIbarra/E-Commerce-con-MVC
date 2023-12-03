using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> lista = new List<Categoria>();

            lista = new CN_Categoria().listar();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult ListarMarcaPorCategoria(int idcategoria)
        {
            List<Marca> lista = new List<Marca>();

            lista = new CN_Marca().listarmarcaporcategoria(idcategoria);

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);

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
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(x.RutaImagen,x.NombreImagen), out conversion),
                Extension = Path.GetExtension(x.NombreImagen),
                Activo = x.Activo,

            }).Where(x => x.ObjCategoria.IdCategoria == (idcategoria == 0 ? x.ObjCategoria.IdCategoria : idcategoria) && 
            x.ObjMarca.IdMarca == (idmarca == 0 ? x.ObjMarca.IdMarca : idmarca) && x.Stock > 0 && x.Activo == true
            ).ToList();

            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult;
            

        }


    }
}