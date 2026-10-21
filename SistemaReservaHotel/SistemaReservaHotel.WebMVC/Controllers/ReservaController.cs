using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaReservaHotel.Business.Helpers;
using SistemaReservaHotel.Business.Interfaces;
using SistemaReservaHotel.DataAccess;
using SistemaReservaHotel.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaReservaHotel.WebMVC.Controllers
{
    public class CancelarReservaDto
    {
        public int Id { get; set; }
    }

    public class ReservaController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IReservaService _reservaService;

        public ReservaController(AppDbContext context, IReservaService reservaService)
        {
            _context = context;
            _reservaService = reservaService;
        }

        // POST: /Reserva/Crear
        [HttpPost]
        [Route("Reserva/Crear")]
        public async Task<IActionResult> Crear([FromBody] Reserva? reserva)
        {
            if (reserva == null)
            {
                return BadRequest(new { error = "Los datos de la reserva son requeridos." });
            }

            // US-16 #1040: Validación de reglas cronológicas
            if (!ValidadorFechas.FechasValidas(reserva.FechaEntrada, reserva.FechaSalida))
            {
                return BadRequest(new { error = "Rango de fechas inválido o fechas en el pasado." });
            }

            // US-SEC-01 #1043: Control de colisión y disponibilidad
            var disponible = await _reservaService.ValidarDisponibilidadAsync(
                reserva.HabitacionId, reserva.FechaEntrada, reserva.FechaSalida);

            if (!disponible)
            {
                return BadRequest(new { error = "La habitación seleccionada no se encuentra disponible en las fechas requeridas." });
            }

            if (string.IsNullOrWhiteSpace(reserva.Estado))
            {
                reserva.Estado = "Activa";
            }

            await _context.Reservas.AddAsync(reserva);
            await _context.SaveChangesAsync();

            return Ok(reserva);
        }

        // GET: /Reserva/ObtenerPorDia
        [HttpGet]
        [Route("Reserva/ObtenerPorDia")]
        public async Task<IActionResult> ObtenerPorDia([FromQuery] DateTime? fecha)
        {
            DateTime fechaConsulta = (fecha ?? DateTime.Today).Date;

            var reservas = await _context.Reservas
                .Where(r => r.FechaEntrada.Date <= fechaConsulta && r.FechaSalida.Date >= fechaConsulta)
                .ToListAsync();

            return Ok(reservas);
        }

        // GET: /Reserva/Detalles/{id}
        [HttpGet]
        [Route("Reserva/Detalles/{id}")]
        public async Task<IActionResult> Detalles(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound(new { error = $"Reserva con ID {id} no encontrada." });
            }

            return Ok(reserva);
        }

        // POST: /Reserva/Cancelar
        [HttpPost]
        [Route("Reserva/Cancelar")]
        [Route("Reserva/Cancelar/{id?}")]
        public async Task<IActionResult> Cancelar([FromBody] CancelarReservaDto? dto, [FromRoute] int? id)
        {
            int reservaId = dto?.Id ?? id ?? 0;
            var reserva = await _context.Reservas.FindAsync(reservaId);
            if (reserva == null)
            {
                return NotFound(new { error = $"Reserva con ID {reservaId} no encontrada." });
            }

            reserva.Estado = "Cancelada";
            await _context.SaveChangesAsync();

            return Ok(reserva);
        }
    }
}
