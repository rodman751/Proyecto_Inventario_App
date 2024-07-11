using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Inventario.MVC.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace Inventario.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

                //cokkies
                builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie( config =>
                    {
                        config.Cookie.Name = "auth_token";
                        config.LoginPath = "/Login/Index";
                        config.ExpireTimeSpan = TimeSpan.FromMinutes(1);
                    });

            builder.Services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false; // Deshabilitar el enrutamiento de puntos finales
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.BottomRight; });


            var cultureInfo = new CultureInfo("en-US");
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;



            var app = builder.Build();


            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(cultureInfo),
                SupportedCultures = new List<CultureInfo> { cultureInfo },
                SupportedUICultures = new List<CultureInfo> { cultureInfo }
            });




            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Authentication and authorization middleware
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseMiddleware<AuthenticationMiddleware>();

            // Notyf middleware
            app.UseNotyf();

            // MVC routing
            app.UseMvc();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
