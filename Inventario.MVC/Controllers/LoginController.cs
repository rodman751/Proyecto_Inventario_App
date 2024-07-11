using AspNetCoreHero.ToastNotification.Abstractions;
using Inventario.ConsumeAPI;
using Inventario.MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Infrastructure;


namespace Inventario.MVC.Controllers
{
    public class LoginController : Controller
    {

        public INotyfService _notifyService { get; }

        private string urlLogin;
        private string UrlMyuser;
        

        public LoginController(IConfiguration configuration, INotyfService notyfService)
        {
            urlLogin = configuration.GetValue("UrlLogin", "").ToString() + "/Login";
            UrlMyuser = configuration.GetValue("UrlLogin", "").ToString() + "/Myuser";



            _notifyService = notyfService;
            
        }   
        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task< IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var response = CRUD<LoginResponseViewModel>.Login(urlLogin, model.usr_user, model.usr_password);
                    string token = response.access_token;

                    var data = CRUD<User>.Read_Token(UrlMyuser, token);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, data.usr_user),
                        new Claim("Password", data.usr_password)
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    // Manejo de errores, puedes personalizar el mensaje según tus necesidades
                    return StatusCode(500, $"Error al autenticar: {ex.Message}");   
                }
            }

            return BadRequest(ModelState);
        }

        public async Task< IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }

       
    }
    
}
