using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using MiradorB.Models;
using MiradorB.Servicios.Contrato;
using MiradorB.Servicios.Implementacion;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace MiradorB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<BdMiradorContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Conexion")));

            builder.Services.AddScoped<IClienteService, ClienteService>();

            // Configuraci�n de autenticaci�n con cookies
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Inicio/IniciarSesion";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                });

            // Configuraci�n de autorizaci�n
            builder.Services.AddAuthorization(options =>
            {
                // Definir la pol�tica DashboardPermiso
                options.AddPolicy("DashboardPermiso", policy =>
                    policy.RequireClaim("Permiso", "DashboardAcceso")); // Ajusta el claim seg�n tu l�gica
            });

            // Configuraci�n adicional
            builder.Services.AddControllers().AddFluentValidation(Fv => Fv.RegisterValidatorsFromAssemblyContaining<Program>());

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(
                    new ResponseCacheAttribute
                    {
                        NoStore = true,
                        Location = ResponseCacheLocation.None,
                    }
                );
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            // Agregar autenticaci�n y autorizaci�n
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Inicio}/{action=IniciarSesion}/{id?}");

            app.Run();
        }
    }
}
