using Inventario.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Inventario.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [Authorize]
        public IActionResult Index()
        {
            var userName = HttpContext.User.Identity.Name;
            var rol = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var funcionalidad = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Modulos")?.Value;

            // Construir el mensaje de bienvenida
            var mensajeBienvenida = $"Bienvenido, {userName} ({rol}). Funcionalidad actual: {funcionalidad}";

            // Puedes pasar el mensajeBienvenida como un ViewBag o ViewData
            ViewData["MensajeBienvenida"] = mensajeBienvenida;
            ViewData["NoLayout"] = true;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
        public IActionResult AccessDenied()
        {
            ViewData["NoLayout"] = true;
            return View();
        }


    }
}
