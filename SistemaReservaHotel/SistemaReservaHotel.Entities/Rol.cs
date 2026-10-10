using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaReservaHotel.Entities
{
    public class Rol
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del rol es obligatorio")]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Descripcion { get; set; }

        public bool Activo { get; set; } = true;

        // Relación: Un rol puede tener muchos usuarios
        public ICollection<UsuarioRol> UsuarioRoles { get; set; }
            = new List<UsuarioRol>();

        // Relación: Un rol puede tener muchos permisos
        public ICollection<RolPermiso> RolPermisos { get; set; }
            = new List<RolPermiso>();

        // Propiedad computada
        [NotMapped]
        public IEnumerable<string> NombresPermisos => RolPermisos
            .Where(rp => rp.Permiso != null)
            .Select(rp => rp.Permiso!.Nombre);
    }
}