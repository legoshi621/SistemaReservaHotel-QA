using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaReservaHotel.Entities
{
    public class MetodoPago
    {
        [Key]
        public int MetodoPagoId { get; set; }

        [Required]
        public string Nombre { get; set; }

        public ICollection<Pago> Pagos { get; set; }
    }
}