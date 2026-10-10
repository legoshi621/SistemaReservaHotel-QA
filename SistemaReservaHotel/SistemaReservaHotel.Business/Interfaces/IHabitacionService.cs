using SistemaReservaHotel.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaReservaHotel.Business.Interfaces
{
    public interface IHabitacionService
    {
        Task<List<Habitacion>> ObtenerTodasAsync();
        Task<Habitacion?> ObtenerPorIdAsync(int id);
        Task<List<Habitacion>> ObtenerDisponiblesAsync(DateTime entrada, DateTime salida);
    }
}
