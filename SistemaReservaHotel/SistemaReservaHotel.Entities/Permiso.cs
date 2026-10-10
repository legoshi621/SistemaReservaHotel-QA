using SistemaReservaHotel.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaReservaHotel.Entities
{
    public class Permiso
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del permiso es obligatorio")]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Descripcion { get; set; }

        [MaxLength(50)]
        public string? Categoria { get; set; }
        // ej: "Ventas", "Productos", "Reportes", "Administracion"

        // Relación: Un permiso puede estar en muchos roles
        public ICollection<RolPermiso> RolPermisos { get; set; }
            = new List<RolPermiso>();
    }
}