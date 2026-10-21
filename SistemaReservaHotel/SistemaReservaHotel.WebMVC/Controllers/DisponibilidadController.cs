using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaReservaHotel.DataAccess;
using SistemaReservaHotel.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaReservaHotel.WebMVC.Controllers
{
    public class DisponibilidadController : Controller
    {
        private readonly AppDbContext _context;

        public DisponibilidadController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Disponibilidad
        [HttpGet]
        [Route("Disponibilidad")]
        public async Task<IActionResult> Index([FromQuery] DateTime? entrada, [FromQuery] DateTime? salida)
        {
            DateTime fechaEntrada = (entrada ?? DateTime.Today).Date;
            DateTime fechaSalida = (salida ?? DateTime.Today.AddDays(1)).Date;

            var habitacionesOcupadas = await _context.Reservas
                .Where(r => r.Estado == "Activa" &&
                            fechaEntrada < r.FechaSalida.Date &&
                            fechaSalida > r.FechaEntrada.Date)
                .Select(r => r.HabitacionId)
                .Distinct()
                .ToListAsync();

            var disponibles = await _context.Habitaciones
                .Where(h => h.Activa && !habitacionesOcupadas.Contains(h.Id))
                .ToListAsync();

            return Ok(disponibles);
        }
    }
}
