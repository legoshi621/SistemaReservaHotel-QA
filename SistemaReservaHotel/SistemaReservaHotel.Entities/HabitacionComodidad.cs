using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaReservaHotel.Entities
{
    public class HabitacionComodidad
    {
        public int HabitacionId { get; set; }
        public int ComodidadId { get; set; }

        [ForeignKey("HabitacionId")]
        public Habitacion habitacion { get; set; }

        [ForeignKey("ComodidadId")]
        public Comodidad Comodidad { get; set; }
    }
}
