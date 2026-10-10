using SistemaReservaHotel.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaReservaHotel.DataAccess.Repositories
{
    public class ComodidadRepository
    {
        private static List<Comodidad> comodidades = new List<Comodidad>
        {
            new Comodidad { Id = 1, Nombre = "WiFi", Icono = "wifi" },
            new Comodidad { Id = 2, Nombre = "TV", Icono = "tv" },
            new Comodidad { Id = 3, Nombre = "Aire acondicionado", Icono = "ac_unit" },
            new Comodidad { Id = 4, Nombre = "Jacuzzi", Icono = "hot_tub" }
        };

        public Task<List<Comodidad>> ObtenerTodasAsync()
        {
            return Task.FromResult(comodidades);
        }

        public Task<Comodidad?> ObtenerPorIdAsync(int id)
        {
            var comodidad = comodidades.Find(c => c.Id == id);
            return Task.FromResult(comodidad);
        }
    }
}
