namespace Inventario.MVC.Controllers
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Lógica de autenticación o cualquier otra lógica que necesites
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Response.Redirect("/LoginController/Index");
                return;
            }

            await _next(context);
        }
    }
}
