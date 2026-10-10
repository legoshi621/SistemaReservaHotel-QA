using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaReservaHotel.Entities
{
    public class TipoHabitacion
    {
        [Key]
        public int TipoHabitacionId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public ICollection<Habitacion> Habitaciones { get; set; }
        public ICollection<TemporadaPrecio> TemporadaPrecios { get; set; }
    }
}