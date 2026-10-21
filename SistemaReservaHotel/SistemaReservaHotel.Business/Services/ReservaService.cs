using SistemaReservaHotel.Business.Interfaces;
using SistemaReservaHotel.DataAccess.Repositories;
using SistemaReservaHotel.Entities;

namespace SistemaReservaHotel.Business.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _reservaRepository;

        public ReservaService(IReservaRepository reservaRepository)
        {
            _reservaRepository = reservaRepository;
        }

        public async Task<List<Reserva>> ObtenerTodasAsync()
        {
            return await _reservaRepository.ObtenerTodasAsync();
        }

        public async Task<Reserva?> ObtenerPorIdAsync(int id)
        {
            return await _reservaRepository.ObtenerPorIdAsync(id);
        }

        public async Task<Reserva> ReservarAsync(Reserva reserva)
        {
            return await _reservaRepository.CrearAsync(reserva);
        }

        public async Task ActualizarAsync(Reserva reserva)
        {
            await _reservaRepository.ActualizarAsync(reserva);
        }

        public async Task EliminarAsync(int id)
        {
            await _reservaRepository.EliminarAsync(id);
        }

        public async Task<bool> ValidarDisponibilidadAsync(int habitacionId, DateTime entrada, DateTime salida)
        {
            return await _reservaRepository
                .ValidarDisponibilidadAsync(habitacionId, entrada, salida);
        }

        public async Task<List<Reserva>> ObtenerPorDiaAsync(DateTime fecha)
        {
            return await _reservaRepository.ObtenerPorDiaAsync(fecha);
        }
    }
}