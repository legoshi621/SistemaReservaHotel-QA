using Microsoft.EntityFrameworkCore;
using SistemaReservaHotel.Business.Interfaces; // Esto conecta con la interfaz
using SistemaReservaHotel.DataAccess;
using SistemaReservaHotel.Entities;

namespace SistemaReservaHotel.Business.Services
{
    public class HabitacionService : IHabitacionService
    {
        private readonly AppDbContext _context;

        public HabitacionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Habitacion>> ObtenerTodasAsync()
        {
            return await _context.Habitaciones.ToListAsync();
        }

        public async Task<Habitacion> ObtenerPorIdAsync(int id)
        {
            return await _context.Habitaciones.FindAsync(id);
        }

        public async Task<List<Habitacion>> ObtenerDisponiblesAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            // Lógica básica para que no de error
            return await _context.Habitaciones.ToListAsync();
        }
    }
}