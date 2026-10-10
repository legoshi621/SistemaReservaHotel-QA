using System.ComponentModel.DataAnnotations;

namespace SistemaReservaHotel.Business.DTOs
{
    public class RolDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del rol es obligatorio")]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Descripcion { get; set; }

        public bool Activo { get; set; } = true;

        // Lista de IDs de permisos asignados
        public List<int> PermisosIds { get; set; } = new();

        // Lista de nombres de permisos (para mostrar)
        public List<string> PermisosNombres { get; set; } = new();
    }
}