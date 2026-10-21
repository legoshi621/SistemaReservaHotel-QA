using SistemaReservaHotel.Business.Interfaces;
using SistemaReservaHotel.DataAccess.Repositories;
using SistemaReservaHotel.Entities;

namespace SistemaReservaHotel.Business.Services
{
    public class DisponibilidadService : IDisponibilidadService
    {
        private readonly IHabitacionRepository _habitacionRepository;
        private readonly IReservaRepository _reservaRepository;

        public DisponibilidadService(
            IHabitacionRepository habitacionRepository,
            IReservaRepository reservaRepository)
        {
            _habitacionRepository = habitacionRepository;
            _reservaRepository = reservaRepository;
        }

        public async Task<List<Habitacion>> ObtenerDisponiblesAsync(DateTime entrada, DateTime salida)
        {
            entrada = entrada.Date;
            salida = salida.Date;

            var habitaciones = await _habitacionRepository.ObtenerTodasAsync();
            var reservas = await _reservaRepository.ObtenerTodasAsync();

            var habitacionesOcupadas = reservas
                .Where(r =>
                    r.Estado == "Activa" &&
                    entrada < r.FechaSalida.Date &&
                    salida > r.FechaEntrada.Date)
                .Select(r => r.HabitacionId)
                .Distinct()
                .ToList();

            var disponibles = habitaciones
                .Where(h => h.Activa && !habitacionesOcupadas.Contains(h.Id))
                .ToList();

            return disponibles;
        }
    }
}