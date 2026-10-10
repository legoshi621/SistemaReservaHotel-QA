namespace SistemaReservaHotel.Entities
{
    public class Reserva
    {
        public int Id { get; set; }

        public int HabitacionId { get; set; }

        public string NombreCliente { get; set; } = string.Empty;

        public DateTime FechaEntrada { get; set; }

        public DateTime FechaSalida { get; set; }

        public decimal PrecioTotal { get; set; }

        public string Estado { get; set; } = "Activa";

        public int UsuarioId { get; set; } // opcional si manejas usuarios
        public int Noches => (FechaSalida - FechaEntrada).Days;

    }
}