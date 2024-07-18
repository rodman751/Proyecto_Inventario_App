using AspNetCoreHero.ToastNotification.Abstractions;
using Inventario.ConsumeAPI;
using Inventario.Entidades;
using Inventario.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace Inventario.MVC.Controllers
{
    public class ProductosController : Controller
    {
        public INotyfService _notifyService { get; }
        private string Productos;
        private string audit;
        public ProductosController(IConfiguration configuration, INotyfService notyfService)
        {
            Productos = configuration.GetValue("ApiUrlBase", "").ToString() + "/Producto";
            audit = configuration.GetValue("UrlLogin", "").ToString() + "/auditoria";
            _notifyService = notyfService;
        }
        // GET: ProductosController
        [Authorize(Roles = "INV-ADMIN,INV-BODEGUERO")]
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5;
            var data = CRUD<Producto>.Read(Productos).ToPagedList(pageNumber, pageSize);
            return View(data);
        }

        // GET: ProductosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductosController/Create
        [Authorize(Roles = "INV-ADMIN,INV-BODEGUERO")]
        public ActionResult _CreatePartial()
        {
             return View();
            //return PartialView("_CreatePartial");
        }

        // POST: ProductosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Producto collection)
        {
            try
            {
                var userName = HttpContext.User.Identity.Name;
                var modulo = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Modulos")?.Value;
                var auditdata = new auditoria
                {

                    aud_usuario = userName,
                    aud_accion = "Create",
                    aud_modulo = "Inventario",
                    aud_funcionalidad = modulo,
                    aud_observacion = " "
                };
                var auditresponse = CRUD<auditoria>.Created(audit, auditdata);

                var pvp = collection.Costo;
                if(collection.GravaIVA == true)
                {
                    pvp = pvp +(pvp * 0.15m);
                    collection.PVP = pvp;
                }
                else
                {
                    collection.PVP = pvp;
                }
                
                var data = CRUD<Producto>.Created(Productos, collection);
                _notifyService.Success("Producto creado");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductosController/Edit/5
        [Authorize(Roles = "INV-ADMIN,INV-BODEGUERO")]
        public ActionResult EditPartial(int id)
        {
            var data = CRUD<Producto>.Read_ById(Productos,id);

            //return View(data);
            return PartialView("_EditPartial", data);
        }

        // POST: ProductosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Producto collection)
        {
            try
            {
                var userName = HttpContext.User.Identity.Name;
                var modulo = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Modulos")?.Value;
                var auditdata = new auditoria
                {

                    aud_usuario = userName,
                    aud_accion = "Update",
                    aud_modulo = "Inventario",
                    aud_funcionalidad = modulo,
                    aud_observacion = " "
                };
                var auditresponse = CRUD<auditoria>.Created(audit, auditdata);

                var pvp = collection.Costo;
                if (collection.GravaIVA == true)
                {
                    pvp = pvp + (pvp * 0.15m);
                    collection.PVP = pvp;
                }
                else
                {
                    collection.PVP = pvp;
                }
                var data = CRUD<Producto>.Update(Productos, id, collection);

                _notifyService.Information("Producto actualizado");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductosController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: ProductosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
           
            try
            {
                var userName = HttpContext.User.Identity.Name;
                var modulo = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Modulos")?.Value;
                var auditdata = new auditoria
                {

                    aud_usuario = userName,
                    aud_accion = "Delete",
                    aud_modulo = "Inventario",
                    aud_funcionalidad = modulo,
                    aud_observacion = " "
                };
                var auditresponse = CRUD<auditoria>.Created(audit, auditdata);
                var data = CRUD<Producto>.Delete(Productos, id);
                _notifyService.Information("Producto eliminado");
                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

       
    }
}
