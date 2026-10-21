using SistemaReservaHotel.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaReservaHotel.DataAccess.Repositories
{
    public interface IReservaRepository
    {
        Task<List<Reserva>> ObtenerTodasAsync();
        Task<Reserva?> ObtenerPorIdAsync(int id);
        Task<List<Reserva>> ObtenerPorDiaAsync(DateTime fecha);
        Task<Reserva> CrearAsync(Reserva reserva);
        Task ActualizarAsync(Reserva reserva);
        Task EliminarAsync(int id);
        Task<bool> ValidarDisponibilidadAsync(int habitacionId, DateTime entrada, DateTime salida);
    }
}
