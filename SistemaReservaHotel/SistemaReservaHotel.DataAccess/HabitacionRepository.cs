using SistemaReservaHotel.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaReservaHotel.DataAccess.Repositories
{
    public class HabitacionRepository
    {
        private static List<Habitacion> habitaciones = new List<Habitacion>
        {
            new Habitacion
            {
                Id = 1,
                Numero = 101,
                Tipo = "Individual",
                Descripcion = "Habitación cómoda para una persona.",
                PrecioBase = 50,
                Capacidad = 1,
                Tamaño = 20,
                ImagenUrl = "https://picsum.photos/800/400?random=1",
                Activa = true,
                Estado = "Disponible",
                Comodidades = new List<string> { "WiFi", "TV", "Aire acondicionado" }
            },
            new Habitacion
            {
                Id = 2,
                Numero = 102,
                Tipo = "Doble",
                Descripcion = "Habitación perfecta para parejas.",
                PrecioBase = 80,
                Capacidad = 2,
                Tamaño = 30,
                ImagenUrl = "https://picsum.photos/800/400?random=2",
                Activa = true,
                Estado = "Disponible",
                Comodidades = new List<string> { "WiFi", "TV", "Mini bar", "Balcón" }
            },
            new Habitacion
            {
                Id = 3,
                Numero = 201,
                Tipo = "Suite",
                Descripcion = "Suite de lujo con vista panorámica.",
                PrecioBase = 150,
                Capacidad = 4,
                Tamaño = 50,
                ImagenUrl = "https://picsum.photos/800/400?random=3",
                Activa = false,
                Estado = "No disponible",
                Comodidades = new List<string> { "WiFi", "TV 55\"", "Jacuzzi", "Sala privada" }
            }
        };

        public Task<List<Habitacion>> ObtenerTodasAsync()
        {
            return Task.FromResult(habitaciones);
        }

        public Task<Habitacion?> ObtenerPorIdAsync(int id)
        {
            var habitacion = habitaciones.FirstOrDefault(h => h.Id == id);
            return Task.FromResult(habitacion);
        }

        public Task<List<Habitacion>> ObtenerDisponiblesAsync()
        {
            var disponibles = habitaciones.Where(h => h.Activa && h.Estado == "Disponible").ToList();
            return Task.FromResult(disponibles);
        }
    }
}
