
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SistemaReservaHotel.Entities;

namespace SistemaReservaHotel.Entities
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [MaxLength(100)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? UltimoAcceso { get; set; }

        [MaxLength(20)]
        public string? Telefono { get; set; }

        // Relación: Un usuario puede tener muchos roles
        public ICollection<UsuarioRol> UsuarioRoles { get; set; }
            = new List<UsuarioRol>();

        // Propiedad computada: obtener nombres de roles
        [NotMapped]
        public IEnumerable<string> NombresRoles => UsuarioRoles
            .Where(ur => ur.Rol != null)
            .Select(ur => ur.Rol!.Nombre);
    }
}