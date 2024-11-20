using Microsoft.AspNetCore.Mvc;
using MiradorB.Models;
using MiradorB.Recursos;
using MiradorB.Servicios.Contrato;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using MiradorB.Servicios.Implementacion;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace MiradorB.Controllers
{
    public class InicioController : Controller
    {
        private readonly IClienteService _clienteServicio;
        private readonly EmailSender _emailSender; // Instancia de EmailSender
        private static Dictionary<string, string> _recoveryCodes = new Dictionary<string, string>(); // Almacenamiento temporal de códigos de recuperación

        public InicioController(IClienteService clienteServicio)
        {
            _clienteServicio = clienteServicio;
            _emailSender = new EmailSender(); // Inicialización de EmailSender
        }

        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(Cliente modelo)
        {
            modelo.Contrasena = Utilidades.EncriptarClave(modelo.Contrasena);

            Cliente cliente_creado = await _clienteServicio.SaveCliente(modelo);

            if (cliente_creado.NroDocumento > 0)
            {
                // Enviar correo de confirmación de registro
                string subject = "Bienvenido a nuestra aplicación";
                string message = "<h1>Registro exitoso</h1><p>Gracias por registrarte en nuestra aplicación.</p>";

                await _emailSender.SendEmailAsync(cliente_creado.Correo, subject, message);

                return RedirectToAction("IniciarSesion", "Inicio");
            }

            ViewData["mensaje"] = "No se pudo crear el cliente";
            return View();
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string correo, string contrasena)
        {
            Cliente cliente_encontrado = await _clienteServicio.GetCliente(correo, Utilidades.EncriptarClave(contrasena));

            if (cliente_encontrado == null)
            {
                ViewData["mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, cliente_encontrado.Nombres)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            // Enviar correo de confirmación de inicio de sesión
            string subject = "Inicio de sesión exitoso";
            string message = "<h1>¡Bienvenido de nuevo!</h1><p>Has iniciado sesión correctamente.</p>";

            await _emailSender.SendEmailAsync(correo, subject, message);

            return RedirectToAction("Index", "Home");
        }

        // Acción para solicitar el código de recuperación de contraseña
        public IActionResult RecuperarContrasena()
        {
            return View("~/Views/Shared/RecuperarContrasena.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> RecuperarContrasena(string correo)
        {
            // Validar si el correo existe en la base de datos
            Cliente cliente = await _clienteServicio.GetClienteByEmail(correo);

            if (cliente == null)
            {
                // Si no se encuentra el correo, mostrar mensaje de error
                ViewData["mensaje"] = "Correo no encontrado";
                return View();
            }

            // Generar un código de 6 dígitos para la recuperación
            var code = new Random().Next(100000, 999999).ToString();

            // Guardar el código temporalmente en el diccionario (puedes almacenar esto en la base de datos si es necesario)
            _recoveryCodes[correo] = code;

            // Enviar el código al correo del usuario
            await _emailSender.SendRecoveryCodeAsync(correo, code);

            // Redirigir a la vista de verificación de código
            TempData["correo"] = correo;
            return RedirectToAction("VerificarCodigo", "Inicio");
        }

        // Acción para mostrar el formulario de verificación de código
        public IActionResult VerificarCodigo()
        {
            return View("~/Views/Shared/VerificarCodigo.cshtml");
        }

        [HttpPost]
        public IActionResult VerificarCodigo(string correo, string codigo)
        {
            // Verificar si el código ingresado es correcto
            if (_recoveryCodes.ContainsKey(correo) && _recoveryCodes[correo] == codigo)
            {
                // Código correcto; permitir restablecer contraseña
                _recoveryCodes.Remove(correo); // Remover el código después de usarlo
                TempData["correo"] = correo;
                return RedirectToAction("RestablecerContrasena", "Inicio");
            }

            ViewData["mensaje"] = "Código incorrecto";
            return View();
        }

        // Acción para mostrar el formulario de restablecimiento de contraseña
        public IActionResult RestablecerContrasena()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RestablecerContrasena(string correo, string nuevaContrasena)
        {
            // Validar que el correo y la nueva contraseña no estén vacíos
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(nuevaContrasena))
            {
                ViewData["mensaje"] = "Correo o contraseña no válidos.";
                return View();
            }

            // Obtener al cliente usando el correo proporcionado
            Cliente cliente = await _clienteServicio.GetClienteByEmail(correo);

            if (cliente == null)
            {
                ViewData["mensaje"] = "No se encontró el cliente.";
                return View();
            }

            // Encriptar la nueva contraseña
            cliente.Contrasena = Utilidades.EncriptarClave(nuevaContrasena);

            // Actualizar el cliente con la nueva contraseña
            bool actualizado = await _clienteServicio.UpdateCliente(cliente);

            if (actualizado)
            {
                // Si la actualización fue exitosa, mostrar un mensaje de éxito
                TempData["mensaje"] = "Contraseña restablecida con éxito.";
                return RedirectToAction("IniciarSesion", "Inicio");
            }

            // Si hubo un problema al actualizar, mostrar un mensaje de error
            ViewData["mensaje"] = "No se pudo restablecer la contraseña.";
            return View();
        }
    }
}