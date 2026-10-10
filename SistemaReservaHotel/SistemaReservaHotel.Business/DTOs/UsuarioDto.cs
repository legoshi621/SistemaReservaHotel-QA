using System.ComponentModel.DataAnnotations;

namespace SistemaReservaHotel.Business.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [StringLength(100)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Telefono { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; }

        public DateTime? UltimoAcceso { get; set; }

        // Lista de nombres de roles asignados
        public List<string> Roles { get; set; } = new();
    }
}
