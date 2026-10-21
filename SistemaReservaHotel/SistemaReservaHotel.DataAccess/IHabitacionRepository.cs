using SistemaReservaHotel.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaReservaHotel.DataAccess.Repositories
{
    public interface IHabitacionRepository
    {
        Task<List<Habitacion>> ObtenerTodasAsync();
        Task<Habitacion?> ObtenerPorIdAsync(int id);
        Task<List<Habitacion>> ObtenerDisponiblesAsync();
    }
}
