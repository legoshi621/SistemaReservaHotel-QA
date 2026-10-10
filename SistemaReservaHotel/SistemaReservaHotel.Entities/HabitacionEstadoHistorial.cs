using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaReservaHotel.Entities
{
    public class HabitacionEstadoHistorial
    {
        [Key]
        public int HabitacionEstadoHistorialId { get; set; }

        public string Estado { get; set; }

        public DateTime FechaCambio { get; set; }

        public int HabitacionId { get; set; }

        [ForeignKey("HabitacionId")]
        public Habitacion Habitacion { get; set; }
    }
}