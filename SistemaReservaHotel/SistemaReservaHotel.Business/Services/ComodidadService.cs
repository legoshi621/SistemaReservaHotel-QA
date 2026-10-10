using SistemaReservaHotel.Entities;
using SistemaReservaHotel.Business.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaReservaHotel.Business.Services
{
    public class ComodidadService : IComodidadService
    {
        public Task<List<Comodidad>> ObtenerTodasAsync()
        {
            var comodidades = new List<Comodidad>
            {
                new Comodidad { Id = 1, Nombre = "WiFi", Icono = "wifi" },
                new Comodidad { Id = 2, Nombre = "TV", Icono = "tv" },
                new Comodidad { Id = 3, Nombre = "Aire acondicionado", Icono = "ac_unit" },
                new Comodidad { Id = 4, Nombre = "Jacuzzi", Icono = "hot_tub" }
            };

            return Task.FromResult(comodidades);
        }

        public Task<Comodidad?> ObtenerPorIdAsync(int id)
        {
            var comodidades = new List<Comodidad>
            {
                new Comodidad { Id = 1, Nombre = "WiFi", Icono = "wifi" },
                new Comodidad { Id = 2, Nombre = "TV", Icono = "tv" },
                new Comodidad { Id = 3, Nombre = "Aire acondicionado", Icono = "ac_unit" },
                new Comodidad { Id = 4, Nombre = "Jacuzzi", Icono = "hot_tub" }
            };

            var comodidad = comodidades.Find(c => c.Id == id);
            return Task.FromResult(comodidad);
        }
    }
}
