using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SistemaReservaHotel.Business.Services;
using SistemaReservaHotel.Business.DTOs;

namespace SistemaReservaHotel.WebMVC.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IRolService _rolService;

        public AdminController(
            IUsuarioService usuarioService,
            IRolService rolService)
        {
            _usuarioService = usuarioService;
            _rolService = rolService;
        }

        // ═══════════════════════════════════════════════════════════
        // DASHBOARD PRINCIPAL
        // ═══════════════════════════════════════════════════════════
        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml");
        }

        // ═══════════════════════════════════════════════════════════
        // LISTA DE USUARIOS
        // ═══════════════════════════════════════════════════════════
        [HttpGet]
        public async Task<IActionResult> Usuarios()
        {
            var usuarios = await _usuarioService.GetAllAsync();
            return View(usuarios);
        }

        // ═══════════════════════════════════════════════════════════
        // CREAR USUARIO - GET
        // ═══════════════════════════════════════════════════════════
        [HttpGet]
        public async Task<IActionResult> CreateUsuario()
        {
            ViewBag.Roles = await _rolService.GetAllAsync();
            return View();
        }
        // ═══════════════════════════════════════════════════════════
        // CREAR USUARIO - POST
        // ═══════════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUsuario(
            UsuarioDto dto, string password, List<int> rolesIds)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _rolService.GetAllAsync();
                return View(dto);
            }

            var (success, message) = await _usuarioService
                .CreateAsync(dto, password);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                ViewBag.Roles = await _rolService.GetAllAsync();
                return View(dto);
            }

            // Asignar roles seleccionados
            var usuario = (await _usuarioService.GetAllAsync())
                .FirstOrDefault(u => u.Email == dto.Email);

            if (usuario != null && rolesIds != null && rolesIds.Any())
            {
                foreach (var rolId in rolesIds)
                {
                    await _usuarioService.AsignarRolAsync(usuario.Id, rolId);
                }
            }

            TempData["Success"] = message;
            return RedirectToAction(nameof(Usuarios));
        }

        // ═══════════════════════════════════════════════════════════
        // EDITAR USUARIO - GET
        // ═══════════════════════════════════════════════════════════
        [HttpGet]
        public async Task<IActionResult> EditUsuario(int id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            ViewBag.Roles = await _rolService.GetAllAsync();
            return View(usuario);
        }
        // ═══════════════════════════════════════════════════════════
        // EDITAR USUARIO - POST
        // ═══════════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUsuario(UsuarioDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _rolService.GetAllAsync();
                return View(dto);
            }

            var (success, message) = await _usuarioService
                .UpdateAsync(dto);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                ViewBag.Roles = await _rolService.GetAllAsync();
                return View(dto);
            }

            TempData["Success"] = message;
            return RedirectToAction(nameof(Usuarios));
        }

        // ═══════════════════════════════════════════════════════════
        // DESACTIVAR USUARIO
        // ═══════════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var (success, message) = await _usuarioService
                .DeleteAsync(id);

            if (success)
            {
                TempData["Success"] = message;
            }
            else
            {
                TempData["Error"] = message;
            }

            return RedirectToAction(nameof(Usuarios));
        }
        // ═══════════════════════════════════════════════════════════
        // ASIGNAR ROL A USUARIO
        // ═══════════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AsignarRol(
            int usuarioId, int rolId)
        {
            var (success, message) = await _usuarioService
                .AsignarRolAsync(usuarioId, rolId);

            if (success)
            {
                TempData["Success"] = message;
            }
            else
            {
                TempData["Error"] = message;
            }

            return RedirectToAction(nameof(EditUsuario), new { id = usuarioId });
        }

        // ═══════════════════════════════════════════════════════════
        // REMOVER ROL DE USUARIO
        // ═══════════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoverRol(
            int usuarioId, int rolId)
        {
            var (success, message) = await _usuarioService
                .RemoverRolAsync(usuarioId, rolId);

            if (success)
            {
                TempData["Success"] = message;
            }
            else
            {
                TempData["Error"] = message;
            }

            return RedirectToAction(nameof(EditUsuario), new { id = usuarioId });
        }
    }
}
