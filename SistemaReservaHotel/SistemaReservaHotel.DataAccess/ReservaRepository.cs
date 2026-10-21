using Microsoft.EntityFrameworkCore;
using SistemaReservaHotel.DataAccess;
using SistemaReservaHotel.Entities;

namespace SistemaReservaHotel.DataAccess.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly AppDbContext? _context;

        public ReservaRepository()
        {
        }

        public ReservaRepository(AppDbContext context)
        {
            _context = context;
        }

        public virtual async Task<List<Reserva>> ObtenerTodasAsync()
        {
            return _context != null ? await _context.Reservas.ToListAsync() : new List<Reserva>();
        }

        public virtual async Task<Reserva?> ObtenerPorIdAsync(int id)
        {
            return _context != null ? await _context.Reservas.FindAsync(id) : null;
        }

        public virtual async Task<List<Reserva>> ObtenerPorDiaAsync(DateTime fecha)
        {
            if (_context == null) return new List<Reserva>();
            return await _context.Reservas
                .Where(r => r.FechaEntrada <= fecha && r.FechaSalida >= fecha)
                .ToListAsync();
        }

        public virtual async Task<Reserva> CrearAsync(Reserva reserva)
        {
            if (_context != null)
            {
                _context.Reservas.Add(reserva);
                await _context.SaveChangesAsync();
            }
            return reserva;
        }

        public virtual async Task ActualizarAsync(Reserva reserva)
        {
            if (_context != null)
            {
                _context.Reservas.Update(reserva);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task EliminarAsync(int id)
        {
            if (_context != null)
            {
                var reserva = await _context.Reservas.FindAsync(id);
                if (reserva != null)
                {
                    _context.Reservas.Remove(reserva);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public virtual async Task<bool> ValidarDisponibilidadAsync(int habitacionId, DateTime entrada, DateTime salida)
        {
            if (_context == null) return true;
            return !await _context.Reservas.AnyAsync(r =>
                r.HabitacionId == habitacionId &&
                r.Estado == "Activa" &&
                entrada < r.FechaSalida &&
                salida > r.FechaEntrada);
        }
    }
}