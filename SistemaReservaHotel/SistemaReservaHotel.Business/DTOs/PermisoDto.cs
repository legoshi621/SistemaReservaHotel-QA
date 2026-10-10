using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaReservaHotel.Business.DTOs
{
    public class PermisoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? Categoria { get; set; }
    }
}
