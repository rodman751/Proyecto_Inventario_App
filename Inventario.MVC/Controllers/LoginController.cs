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
using Microsoft.CodeAnalysis.Elfie.Serialization;


namespace Inventario.MVC.Controllers
{
    public class LoginController : Controller
    {

        public INotyfService _notifyService { get; }

        private string urlLogin;
        private string UrlMyuser;
        private string urlGet_rol;
        private string getmodulo;
        private string audit;


        public LoginController(IConfiguration configuration, INotyfService notyfService)
        {
            urlLogin = configuration.GetValue("UrlLogin", "").ToString() + "/Login";
            UrlMyuser = configuration.GetValue("UrlLogin", "").ToString() + "/Myuser";
            urlGet_rol = configuration.GetValue("UrlLogin", "").ToString() + "/role_users/user";
            getmodulo = configuration.GetValue("UrlLogin", "").ToString() + "/login_module";
            audit = configuration.GetValue("UrlLogin", "").ToString() + "/auditoria";

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
                    string mod = "Inventario";

                    var data = CRUD<User>.Read_Token(UrlMyuser, token);
                    var rol = CRUD<RoleResponse>.Read_Token_getROL(urlGet_rol, token,data.usr_id);
                    var role = rol.tb_role_user_user.First();
                    var modulos = CRUD<modulo>.Login2(getmodulo, model.usr_user, model.usr_password, mod);


                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, data.usr_user),
                        new Claim(ClaimTypes.Name, data.usr_email),
                        new ("Rol", role.role_name),
                        new Claim( ClaimTypes.Role, role.role_name),
                        new Claim("Token", token),
                        new Claim("Modulos", modulos.functionalities.First())

                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    var userName = HttpContext.User.Identity.Name;
                    var modulo = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Modulos")?.Value;
                    var auditdata = new auditoria
                    {

                        aud_usuario = userName,
                        aud_accion = "Login",
                        aud_modulo = "Inventario",
                        aud_funcionalidad = modulo,
                        aud_observacion = " "
                    };
                    var auditresponse = CRUD<auditoria>.Created(audit, auditdata);

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    // Manejo de errores, puedes personalizar el mensaje según tus necesidades
                    //return StatusCode(500, $"Error al autenticar: {ex.Message}");
                    _notifyService.Error("Error al autenticar: " + "El Usuario no pertenece al Modulo de Inventario");
                    return RedirectToAction("Index", "Login");
                }
            }

            return BadRequest(ModelState);
        }

        public async Task< IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var userName = HttpContext.User.Identity.Name;
            var modulo = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Modulos")?.Value;
            var auditdata = new auditoria
            {

                aud_usuario = userName,
                aud_accion = "Logout",
                aud_modulo = "Inventario",
                aud_funcionalidad = modulo,
                aud_observacion = " "
            };
            var auditresponse = CRUD<auditoria>.Created(audit, auditdata);

            return RedirectToAction("Index", "Login");
        }

       
    }
    
}
