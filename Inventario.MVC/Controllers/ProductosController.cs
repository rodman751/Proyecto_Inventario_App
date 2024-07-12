using AspNetCoreHero.ToastNotification.Abstractions;
using Inventario.ConsumeAPI;
using Inventario.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.MVC.Controllers
{
    public class ProductosController : Controller
    {
        public INotyfService _notifyService { get; }
        private string Productos;
        
        public ProductosController(IConfiguration configuration, INotyfService notyfService)
        {
            Productos = configuration.GetValue("ApiUrlBase", "").ToString() + "/Producto";
            _notifyService = notyfService;
        }
        // GET: ProductosController
        [Authorize(Roles = "Administrador Compras,Bodeguero Inventario")]
        public ActionResult Index()
        {
            
            var data = CRUD<Producto>.Read(Productos);
            return View(data);
        }

        // GET: ProductosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Producto collection)
        {
            try
            {
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
        public ActionResult Edit(int id)
        {
            var data = CRUD<Producto>.Read_ById(Productos,id);

            return View(data);
        }

        // POST: ProductosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Producto collection)
        {
            try
            {
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
