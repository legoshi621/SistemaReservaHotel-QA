using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaReservaHotel.Entities
{
    public class TemporadaPrecio
    {
        [Key]
        public int TemporadaPrecioId { get; set; }

        public decimal Precio { get; set; }

        // FK
        public int TemporadaId { get; set; }
        public int TipoHabitacionId { get; set; }

        [ForeignKey("TemporadaId")]
        public Temporada Temporada { get; set; }

        [ForeignKey("TipoHabitacionId")]
        public TipoHabitacion TipoHabitacion { get; set; }
    }
}
