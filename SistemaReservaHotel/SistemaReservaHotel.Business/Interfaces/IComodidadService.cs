using SistemaReservaHotel.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaReservaHotel.Business.Interfaces
{
    public interface IComodidadService
    {
        Task<List<Comodidad>> ObtenerTodasAsync();
        Task<Comodidad?> ObtenerPorIdAsync(int id);
    }
}
