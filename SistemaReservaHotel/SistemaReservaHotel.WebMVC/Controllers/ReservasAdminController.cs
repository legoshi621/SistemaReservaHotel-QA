using Microsoft.AspNetCore.Mvc;
using SistemaReservaHotel.DataAccess;
using SistemaReservaHotel.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace SistemaReservaHotel.Web.Controllers
{
    public class ReservasAdminController : Controller
    {
        private readonly AppDbContext _context;

        public ReservasAdminController(AppDbContext context)
        {
            _context = context;
        }

        // US-14 VER RESERVAS POR DIA
        public IActionResult PorFecha(DateTime? fecha)
        {
            DateTime fechaConsulta = fecha ?? DateTime.Today;

            var reservas = _context.Reservas
                .Where(r => r.FechaEntrada.Date == fechaConsulta.Date)
                .ToList();

            ViewBag.Fecha = fechaConsulta;

            return View(reservas);
        }

        // FORMULARIO CREAR RESERVA
        public IActionResult Crear()
        {
            ViewBag.TiposHabitacion = _context.TipoHabitaciones.ToList();
            return View();
        }

        // US-16 CREAR RESERVA APLICANDO PRECIO SEGUN TEMPORADA
        [HttpPost]
        public IActionResult Crear(Reserva reserva, int tipoHabitacionId)
        {
            var temporadaPrecio = _context.TemporadasPrecios
                .Include(tp => tp.Temporada)
                .FirstOrDefault(tp =>
                    tp.TipoHabitacionId == tipoHabitacionId);

            if (temporadaPrecio != null)
            {
                reserva.PrecioTotal = temporadaPrecio.Precio;
            }

            _context.Reservas.Add(reserva);
            _context.SaveChanges();

            return RedirectToAction("PorFecha");
        }
    }
}