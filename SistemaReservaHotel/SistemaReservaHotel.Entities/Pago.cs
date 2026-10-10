using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaReservaHotel.Entities
{
    public class Pago
    {
        [Key]
        public int PagoId { get; set; }

        public DateTime FechaPago { get; set; }

        public decimal Monto { get; set; }

        // FK
        public int ReservaId { get; set; }
        public int MetodoPagoId { get; set; }

        [ForeignKey("ReservaId")]
        public Reserva Reserva { get; set; }

        [ForeignKey("MetodoPagoId")]
        public MetodoPago MetodoPago { get; set; }
    }
}