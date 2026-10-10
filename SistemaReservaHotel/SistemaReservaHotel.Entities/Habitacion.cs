namespace SistemaReservaHotel.Entities
{
    public class Habitacion
    {
        public int Id { get; set; }

        public int Numero { get; set; }

        public string Tipo { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public decimal PrecioBase { get; set; }

        public int Capacidad { get; set; }

        public int Tamaño { get; set; }

        public string ImagenUrl { get; set; } = string.Empty;

        public bool Activa { get; set; } = true;


        public List<string> Comodidades { get; set; } = new();

        public string Estado { get; set; } = "Disponible";
        public decimal PrecioActual => PrecioBase; // luego puedes extender con lógica de descuentos

    }
}