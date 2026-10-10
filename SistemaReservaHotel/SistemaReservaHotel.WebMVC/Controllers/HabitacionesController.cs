using Microsoft.AspNetCore.Mvc;
using SistemaReservaHotel.Business.Interfaces;

namespace SistemaReservaHotel.WebMVC.Controllers
{
    public class HabitacionController : Controller
    {
        private readonly IHabitacionService _habitacionService;

        public HabitacionController(IHabitacionService habitacionService)
        {
            _habitacionService = habitacionService;
        }

        // Esta es la acción que busca tu botón del menú
        public async Task<IActionResult> Buscar()
        {
            var habitaciones = await _habitacionService.ObtenerTodasAsync();
            return View(habitaciones);
        }
    }
}