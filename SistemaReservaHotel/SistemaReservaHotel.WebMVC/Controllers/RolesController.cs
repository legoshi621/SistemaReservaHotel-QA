using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SistemaReservaHotel.Business.Services;
using SistemaReservaHotel.Business.DTOs;

namespace SistemaReservaHotel.WebMVC.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class RolesController : Controller
    {
        private readonly IRolService _rolService;

        public RolesController(IRolService rolService)
        {
            _rolService = rolService;
        }

        // ═══════════════════════════════════════════════════════════
        // LISTA DE ROLES
        // ═══════════════════════════════════════════════════════════
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await _rolService.GetAllAsync();
            return View("~/Views/Roles/Index.cshtml", roles);
        }

        // ═══════════════════════════════════════════════════════════
        // CREAR ROL - GET
        // ═══════════════════════════════════════════════════════════
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // ═══════════════════════════════════════════════════════════
        // CREAR ROL - POST
        // ═══════════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RolDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var (success, message) = await _rolService.CreateAsync(dto);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                return View(dto);
            }

            TempData["Success"] = message;
            return RedirectToAction(nameof(Index));
        }
        // ═══════════════════════════════════════════════════════════
        // ASIGNAR PERMISOS - GET
        // ═══════════════════════════════════════════════════════════
        [HttpGet]
        public async Task<IActionResult> AsignarPermisos(int id)
        {
            var rol = await _rolService.GetByIdAsync(id);

            if (rol == null)
            {
                return NotFound();
            }

            var todosPermisos = await _rolService.GetAllPermisosAsync();

            ViewBag.Rol = rol;
            ViewBag.TodosPermisos = todosPermisos;

            return View(rol);
        }

        // ═══════════════════════════════════════════════════════════
        // ASIGNAR PERMISO - POST
        // ═══════════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AsignarPermiso(
            int rolId, int permisoId)
        {
            var (success, message) = await _rolService
                .AsignarPermisoAsync(rolId, permisoId);

            if (success)
            {
                TempData["Success"] = message;
            }
            else
            {
                TempData["Error"] = message;
            }

            return RedirectToAction(nameof(AsignarPermisos), new { id = rolId });
        }
        // ═══════════════════════════════════════════════════════════
        // REMOVER PERMISO - POST
        // ═══════════════════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoverPermiso(
            int rolId, int permisoId)
        {
            var (success, message) = await _rolService
                .RemoverPermisoAsync(rolId, permisoId);

            if (success)
            {
                TempData["Success"] = message;
            }
            else
            {
                TempData["Error"] = message;
            }

            return RedirectToAction(nameof(AsignarPermisos), new { id = rolId });
        }
    }
}
