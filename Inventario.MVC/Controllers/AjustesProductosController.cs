using AspNetCoreHero.ToastNotification.Abstractions;
using Inventario.ConsumeAPI;
using Inventario.Entidades;
using Inventario.Entidades.DTO;
using Inventario.MVC.Models;
using iTextSharp.text.pdf.codec.wmf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using X.PagedList.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Inventario.MVC.Controllers
{
    public class AjustesProductosController : Controller
    {
        public INotyfService _notifyService { get; }

        private string urlApi;
        private string urlApi2;
        private string Productos;
        private string postdetalles;
        private string getCodigo;
        private string audit;
        public AjustesProductosController(IConfiguration configuration, INotyfService notyfService)
        {
            urlApi = configuration.GetValue("ApiUrlBase", "").ToString() + "/AjusteProducto";
            urlApi2 = configuration.GetValue("ApiUrlBase", "").ToString() + "/DetalleAjusteProducto/GetAjusteDetalles";
            Productos = configuration.GetValue("ApiUrlBase", "").ToString() + "/Producto";
            postdetalles= configuration.GetValue("ApiUrlBase", "").ToString() + "/DetalleAjusteProducto";
            getCodigo = configuration.GetValue("ApiUrlBase", "").ToString() + "/AjusteProducto/LastAjuste";
            audit = configuration.GetValue("UrlLogin", "").ToString() + "/auditoria";


            _notifyService = notyfService;
        }
        // GET: AjustesProductosController
        [Authorize(Roles= "INV-ADMIN")]
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5;
            var data = CRUD<AjusteProducto>.Read(urlApi).ToPagedList(pageNumber, pageSize);
            _notifyService.Success("Datos cargados correctamente");
            return View(data);
        }
        [Authorize(Roles= "INV-ADMIN")]
        public async Task<ActionResult> getDetallesSQL(int id)
        {
            var data = await CRUD<DetalleAjusteProductoDTO>.Read_ByIdSQLAsync(urlApi2, id);
            ViewBag.ID_Ajuste = id;
            return View(data);
        }

        // GET: AjustesProductosController/Details/5
        [Authorize(Roles = "INV-ADMIN")]
        public ActionResult Details(int id)
        {
            var data = CRUD<DetalleAjusteProducto>.Read_ById(postdetalles, id);
            int idAjuste = data.ID_Ajuste;
            ViewBag.ID_Ajuste = idAjuste;
            return View(data);
        }

        // GET: AjustesProductosController/Create
        [Authorize(Roles = "INV-ADMIN")]
        public ActionResult CreateDetalleAjusteProducto(int id)
        {
            var productos = CRUD<Producto>.Read(Productos);
            ViewBag.Productos = new SelectList(productos, "ID_Producto", "Nombre");
            ViewBag.ID_Ajuste = id;
            return View();
        }
        [Authorize(Roles = "INV-ADMIN")]
        public ActionResult Edit(int id)
        {
            var data = CRUD<DetalleAjusteProducto>.Read_ById(postdetalles, id);
            var productos = CRUD<Producto>.Read(Productos);
            ViewBag.Productos = new SelectList(productos, "ID_Producto", "Nombre");
            
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DetalleAjusteProducto ajusteProducto, int id)
        {
            try
            {

                //var cantidadAjustada = ajusteProducto.CantidadAjustada;
                //var idProducto = ajusteProducto.ID_Producto;
                //var producto = CRUD<Producto>.Read_ById(Productos, idProducto);
                //var stockActual = producto.StockProducto;

                //if (cantidadAjustada != 0)
                //{
                //    var nuevoStock = stockActual + cantidadAjustada;

                //    if (nuevoStock < 0)
                //    {
                //        // Retorna un error y un mensaje en tu front de stock insuficiente
                //        // Puedes lanzar una excepción, retornar un resultado de error o manejarlo según tu lógica
                //        //throw new InvalidOperationException("Error: No hay suficiente stock para realizar el ajuste.");
                //        _notifyService.Error("Error: No hay suficiente stock para realizar el ajuste.");
                //        return RedirectToAction("Edit", new { id = ajusteProducto.ID_DetalleAjuste });

                //    }
                //    else
                //    {
                //        var productoActualizado = new Producto
                //        {
                //            ID_Producto = idProducto,
                //            Codigo = producto.Codigo,
                //            Nombre = producto.Nombre,
                //            Descripcion = producto.Descripcion,
                //            GravaIVA = producto.GravaIVA,
                //            Costo = producto.Costo,
                //            PVP = producto.PVP,
                //            Estado = producto.Estado,
                //            StockProducto = nuevoStock
                //        };
                //        CRUD<Producto>.Update(Productos, idProducto, productoActualizado);
                //    }
                //}

                int idAjuste = ajusteProducto.ID_Ajuste;
                if (ModelState.IsValid)
                {
                    var userName = HttpContext.User.Identity.Name;
                    var modulo = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Modulos")?.Value;
                    var auditdata = new auditoria
                    {

                        aud_usuario = userName,
                        aud_accion = "Update",
                        aud_modulo = "Inventario",
                        aud_funcionalidad = modulo,
                        aud_observacion = "Update Detalle Ajuste "
                    };
                    var auditresponse = CRUD<auditoria>.Created(audit, auditdata);

                   
                    var newData = CRUD<DetalleAjusteProducto>.Update(postdetalles, id, ajusteProducto);
                    _notifyService.Information("Detalle Ajuste de producto actualizado correctamente");
                    return RedirectToAction("getDetallesSQL", new { id = idAjuste });
                }
                else
                {
                    _notifyService.Error("Error al actualizar el detalle Ajuste de producto");
                    return RedirectToAction("getDetallesSQL", new { id = idAjuste });
                }

            }
            catch
            {
                return View();
            }
        }


        // POST: AjustesProductosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DetalleAjusteProducto ajusteProducto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   
                    var cantidadAjustada = ajusteProducto.CantidadAjustada;
                    var idProducto = ajusteProducto.ID_Producto;
                    var producto = CRUD<Producto>.Read_ById(Productos, idProducto);
                    var stockActual = producto.StockProducto;

                    if (cantidadAjustada != 0)
                    {
                        var nuevoStock = stockActual + cantidadAjustada;

                        if (nuevoStock < 0)
                        {
                            // Retorna un error y un mensaje en tu front de stock insuficiente
                            // Puedes lanzar una excepción, retornar un resultado de error o manejarlo según tu lógica
                            //throw new InvalidOperationException("Error: No hay suficiente stock para realizar el ajuste.");
                            _notifyService.Error("Error: No hay suficiente stock para realizar el ajuste.");
                            return RedirectToAction("CreateDetalleAjusteProducto", new { id = ajusteProducto.ID_Ajuste });

                        }
                        else
                        {
                            var productoActualizado = new Producto
                            {
                                ID_Producto = idProducto,
                                Codigo = producto.Codigo,
                                Nombre = producto.Nombre,
                                Descripcion = producto.Descripcion,
                                GravaIVA = producto.GravaIVA,
                                Costo = producto.Costo,
                                PVP = producto.PVP,
                                Estado = producto.Estado,
                                StockProducto = nuevoStock
                            };
                            CRUD<Producto>.Update(Productos, idProducto, productoActualizado);
                        }
                    }



                    var userName = HttpContext.User.Identity.Name;
                    var modulo = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Modulos")?.Value;
                    var auditdata = new auditoria
                    {

                        aud_usuario = userName,
                        aud_accion = "Create",
                        aud_modulo = "Inventario",
                        aud_funcionalidad = modulo,
                        aud_observacion = "Create new Detalle Ajuste "
                    };
                    var auditresponse = CRUD<auditoria>.Created(audit, auditdata);

 

                    var newData = CRUD<DetalleAjusteProducto>.Created(postdetalles, ajusteProducto);
                    
                    _notifyService.Success("detalle Ajuste de producto creado correctamente");
                    return RedirectToAction("getDetallesSQL", new { id = newData.ID_Ajuste });
                }
                else
                {
                    _notifyService.Error("Error al crear el Empleados");
                    return PartialView("Index", ajusteProducto);
                }

            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles = "INV-ADMIN")]
        public ActionResult NewAjuste( )
        {
            var lastAjuste = CRUD<AjusteProducto>.Read_lastcod(getCodigo);
            string newNumeroAjuste = lastAjuste.NumeroAjuste;
            if (lastAjuste != null)
            {
                // Extraer el número actual y generar el siguiente
                int lastNumber = int.Parse(lastAjuste.NumeroAjuste.Substring(2));
                newNumeroAjuste = "AJ" + (lastNumber + 1).ToString("D3");
            }

            ViewBag.NewNumeroAjuste = newNumeroAjuste;
            return PartialView("_NewAjuste");
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewAjuste(AjusteProducto ajusteProducto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userName = HttpContext.User.Identity.Name;
                    var modulo = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Modulos")?.Value;
                    var auditdata = new auditoria
                    {

                        aud_usuario = userName,
                        aud_accion = "Create",
                        aud_modulo = "Inventario",
                        aud_funcionalidad = modulo,
                        aud_observacion = "Create new Ajuste "
                    };
                    var auditresponse = CRUD<auditoria>.Created(audit, auditdata);

                    var newData = CRUD<AjusteProducto>.Created(urlApi, ajusteProducto);

                    _notifyService.Success(" Ajuste  creado correctamente");
                    return RedirectToAction("Index");
                }
                else
                {
                    _notifyService.Error("Error al crear el Ajuste");
                    return PartialView("Index");
                }

            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {

               

                // Recupera el ajuste de stock asociado
                var pro = CRUD<DetalleAjusteProducto>.Read_ById(postdetalles, id);

                var producto = CRUD<Producto>.Read_ById(Productos, pro.ID_Producto);

                if (producto == null)
                {
                    // Maneja el caso en que el producto no existe
                    return NotFound();
                }
                // Verifica si hay un ajuste que revertir
                if (pro != null)
                {
                    // Calcula el stock original antes del ajuste
                    var stockOriginal = producto.StockProducto - pro.CantidadAjustada;

                    // Actualiza el stock del producto a su valor original
                    producto.StockProducto = stockOriginal;

                    CRUD<Producto>.Update(Productos, pro.ID_Producto, producto);
                }




                //var pro = CRUD<DetalleAjusteProducto>.Read_ById(postdetalles, id);
                var idAjuste = pro.ID_Ajuste;
                if (ModelState.IsValid)
                {
                    var userName = HttpContext.User.Identity.Name;
                    var modulo = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Modulos")?.Value;
                    var auditdata = new auditoria
                    {

                        aud_usuario = userName,
                        aud_accion = "Delete",
                        aud_modulo = "Inventario",
                        aud_funcionalidad = modulo,
                        aud_observacion = "Delete Ajuste"
                    };
                    var auditresponse = CRUD<auditoria>.Created(audit, auditdata);

                    var newData = CRUD<AjusteProducto>.Delete(postdetalles, id);

                    _notifyService.Information(" Ajuste  Eliminado correctamente");
                    return RedirectToAction("getDetallesSQL", new { id = idAjuste });
                }
                else
                {
                    _notifyService.Error("Error al Eliminar el detalle");
                    return RedirectToAction("getDetallesSQL", new { id = idAjuste });
                }

            }
            catch
            {
                return View();
            }
        }


    }
}
