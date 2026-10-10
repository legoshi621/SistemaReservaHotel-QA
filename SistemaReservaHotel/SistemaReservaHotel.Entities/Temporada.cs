using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaReservaHotel.Entities
{
    public class Temporada
    {
        [Key]
        public int TemporadaId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }

        public ICollection<TemporadaPrecio> TemporadaPrecios { get; set; }
    }
}
