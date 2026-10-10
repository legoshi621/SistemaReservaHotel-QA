using Microsoft.AspNetCore.Mvc;
using SistemaReservaHotel.DataAccess;
using System;
using System.Linq;

namespace SistemaReservaHotel.Web.Controllers
{
    public class ReportesController : Controller
    {
        private readonly AppDbContext _context;

        public ReportesController(AppDbContext context)
        {
            _context = context;
        }

        // US-15 REPORTE DE RESERVAS
        public IActionResult Reservas(DateTime? inicio, DateTime? fin)
        {
            var query = _context.Reservas.AsQueryable();

            if (inicio.HasValue && fin.HasValue)
            {
                query = query.Where(r =>
                    r.FechaEntrada >= inicio &&
                    r.FechaSalida <= fin);
            }

            var resultado = query.ToList();

            return View(resultado);
        }
    }
}