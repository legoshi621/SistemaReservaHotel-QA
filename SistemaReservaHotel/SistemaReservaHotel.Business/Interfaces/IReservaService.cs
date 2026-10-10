using SistemaReservaHotel.Entities;

namespace SistemaReservaHotel.Business.Interfaces
{
    public interface IReservaService
    {
        Task<List<Reserva>> ObtenerTodasAsync();
        Task<Reserva?> ObtenerPorIdAsync(int id);
        Task<Reserva> ReservarAsync(Reserva reserva);
        Task ActualizarAsync(Reserva reserva);
        Task EliminarAsync(int id);
        Task<bool> ValidarDisponibilidadAsync(int habitacionId, DateTime entrada, DateTime salida);
        Task<List<Reserva>> ObtenerPorDiaAsync(DateTime fecha);
    }
}