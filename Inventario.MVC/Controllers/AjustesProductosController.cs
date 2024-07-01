using AspNetCoreHero.ToastNotification.Abstractions;
using Inventario.ConsumeAPI;
using Inventario.Entidades;
using Inventario.Entidades.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.MVC.Controllers
{
    public class AjustesProductosController : Controller
    {
        public INotyfService _notifyService { get; }

        private string urlApi;
        private string urlApi2;
        public AjustesProductosController(IConfiguration configuration, INotyfService notyfService)
        {
            urlApi = configuration.GetValue("ApiUrlBase", "").ToString() + "/AjusteProducto";
            urlApi2 = configuration.GetValue("ApiUrlBase", "").ToString() + "/DetalleAjusteProducto/GetAjusteDetalles";
            _notifyService = notyfService;
        }
        // GET: AjustesProductosController
        public ActionResult Index()
        {

            var data = CRUD<AjusteProducto>.Read(urlApi);
            _notifyService.Success("Datos cargados correctamente");
            return View(data);
        }

        public async Task<ActionResult> getDetallesSQL(int id)
        {
           


            var data = await CRUD<DetalleAjusteProductoDTO>.Read_ByIdSQLAsync(urlApi2, id);
            return View(data);
        }

        // GET: AjustesProductosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AjustesProductosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AjustesProductosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AjustesProductosController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AjustesProductosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AjustesProductosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AjustesProductosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
