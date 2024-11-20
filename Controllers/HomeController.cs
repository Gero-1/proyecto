using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiradorB.Models;
using MiradorB.ViewModels;


namespace MiradorB.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly BdMiradorContext _bdContext;

        public HomeController(BdMiradorContext bdContext)
        {
            _bdContext = bdContext;
        }


        public IActionResult Index()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string nombreCliente = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                nombreCliente = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["nombreCliente"] = nombreCliente;


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

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("IniciarSesion", "Inicio");
        }
    }
}
