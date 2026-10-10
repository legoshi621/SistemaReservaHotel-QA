using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using SistemaReservaHotel.Business.Services;
using SistemaReservaHotel.Business.DTOs;

namespace SistemaReservaHotel.WebMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        // ═══════════════════════════════════════════════════════════
        // LOGIN - GET
        // ═══════════════════════════════════════════════════════════
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // ═══════════════════════════════════════════════════════════
        // LOGIN - POST
        // ═══════════════════════════════════════════════════════════
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginDto model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, message, usuario) = await _authService
                .LoginAsync(model);

            if (!success || usuario == null)
            {
                ModelState.AddModelError(string.Empty, message);
                return View(model);
            }
            // Crear claims del usuario
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreCompleto),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            // Agregar roles como claims
            foreach (var rol in usuario.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            // Obtener permisos del usuario
            var permisos = await _authService
                .GetPermisosUsuarioAsync(usuario.Id);

            foreach (var permiso in permisos)
            {
                claims.Add(new Claim("Permiso", permiso));
            }

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Redirigir según returnUrl o al dashboard
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            // Redirigir según rol
            if (usuario.Roles.Contains("Administrador"))
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Home");
        }
        // ═══════════════════════════════════════════════════════════
        // REGISTER - GET
        // ═══════════════════════════════════════════════════════════
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // ═══════════════════════════════════════════════════════════
        // REGISTER - POST
        // ═══════════════════════════════════════════════════════════
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, message) = await _authService
                .RegisterAsync(model, "Usuario");

            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                return View(model);
            }

            TempData["Success"] = message + " Ahora puede iniciar sesión.";
            return RedirectToAction(nameof(Login));
        }

        // ═══════════════════════════════════════════════════════════
        // LOGOUT
        // ═══════════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        // ═══════════════════════════════════════════════════════════
        // ACCESS DENIED
        // ═══════════════════════════════════════════════════════════
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
    

