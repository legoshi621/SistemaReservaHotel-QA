using Microsoft.EntityFrameworkCore;
using SistemaReservaHotel.DataAccess;
using SistemaReservaHotel.Entities;

namespace SistemaReservaHotel.DataAccess.Repositories
{
    public class ReservaRepository
    {
        private readonly AppDbContext _context;

        public ReservaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reserva>> ObtenerTodasAsync()
        {
            return await _context.Reservas.ToListAsync();
        }

        public async Task<Reserva?> ObtenerPorIdAsync(int id)
        {
            return await _context.Reservas.FindAsync(id);
        }

        public async Task<List<Reserva>> ObtenerPorDiaAsync(DateTime fecha)
        {
            return await _context.Reservas
                .Where(r => r.FechaEntrada <= fecha && r.FechaSalida >= fecha)
                .ToListAsync();
        }

        public async Task<Reserva> CrearAsync(Reserva reserva)
        {
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();
            return reserva;
        }

        public async Task ActualizarAsync(Reserva reserva)
        {
            _context.Reservas.Update(reserva);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ValidarDisponibilidadAsync(int habitacionId, DateTime entrada, DateTime salida)
        {
            return !await _context.Reservas.AnyAsync(r =>
                r.HabitacionId == habitacionId &&
                r.Estado == "Activa" &&
                entrada < r.FechaSalida &&
                salida > r.FechaEntrada);
        }
    }
}