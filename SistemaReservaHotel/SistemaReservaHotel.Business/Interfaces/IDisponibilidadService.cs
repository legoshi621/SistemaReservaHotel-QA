using SistemaReservaHotel.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaReservaHotel.Business.Interfaces
{
    public interface IDisponibilidadService
    {
        Task<List<Habitacion>> ObtenerDisponiblesAsync(DateTime entrada, DateTime salida);
    }
}
