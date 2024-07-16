using AspNetCoreHero.ToastNotification.Abstractions;
using Inventario.ConsumeAPI;
using Inventario.Entidades;
using Inventario.Entidades.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.OpenApi.Writers;
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

        public AjustesProductosController(IConfiguration configuration, INotyfService notyfService)
        {
            urlApi = configuration.GetValue("ApiUrlBase", "").ToString() + "/AjusteProducto";
            urlApi2 = configuration.GetValue("ApiUrlBase", "").ToString() + "/DetalleAjusteProducto/GetAjusteDetalles";
            Productos = configuration.GetValue("ApiUrlBase", "").ToString() + "/Producto";
            postdetalles= configuration.GetValue("ApiUrlBase", "").ToString() + "/DetalleAjusteProducto";
            getCodigo = configuration.GetValue("ApiUrlBase", "").ToString() + "/AjusteProducto/LastAjuste";


            _notifyService = notyfService;
        }
        // GET: AjustesProductosController
        //[Authorize(Roles= "INV-ADMIN")]
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5;
            var data = CRUD<AjusteProducto>.Read(urlApi).ToPagedList(pageNumber, pageSize);
            _notifyService.Success("Datos cargados correctamente");
            return View(data);
        }
        //[Authorize(Roles= "INV-ADMIN")]
        public async Task<ActionResult> getDetallesSQL(int id)
        {
            var data = await CRUD<DetalleAjusteProductoDTO>.Read_ByIdSQLAsync(urlApi2, id);
            ViewBag.ID_Ajuste = id;
            return View(data);
        }

        // GET: AjustesProductosController/Details/5
        public ActionResult Details(int id)
        {
            var data = CRUD<DetalleAjusteProducto>.Read_ById(postdetalles, id);
            int idAjuste = data.ID_Ajuste;
            ViewBag.ID_Ajuste = idAjuste;
            return View(data);
        }

        // GET: AjustesProductosController/Create
        public ActionResult CreateDetalleAjusteProducto(int id)
        {
            var productos = CRUD<Producto>.Read(Productos);
            ViewBag.Productos = new SelectList(productos, "ID_Producto", "Nombre");
            ViewBag.ID_Ajuste = id;
            return View();
        }
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
                int idAjuste = ajusteProducto.ID_Ajuste;
                if (ModelState.IsValid)
                {
                    
                    var newData = CRUD<DetalleAjusteProducto>.Update(postdetalles, id, ajusteProducto);
                    _notifyService.Success("Detalle Ajuste de producto actualizado correctamente");
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
                var pro = CRUD<DetalleAjusteProducto>.Read_ById(postdetalles, id);
                var idAjuste = pro.ID_Ajuste;
                if (ModelState.IsValid)
                {

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
