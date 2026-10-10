using SistemaReservaHotel.Business.DTOs;
using SistemaReservaHotel.DataAccess.UnitOfWork;
using SistemaReservaHotel.Entities;

namespace SistemaReservaHotel.Business.Services
{
    public class RolService : IRolService
    {
        private readonly IUnitOfWork _uow;

        public RolService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<RolDto>> GetAllAsync()
        {
            var roles = await _uow.Roles.GetRolesActivosAsync();
            return roles.Select(MapToDto);
        }

        public async Task<RolDto?> GetByIdAsync(int id)
        {
            var rol = await _uow.Roles.GetWithPermisosAsync(id);
            return rol == null ? null : MapToDto(rol);
        }

        public async Task<(bool success, string message)> CreateAsync(RolDto dto)
        {
            var nuevoRol = new Rol
            {
                Nombre = dto.Nombre.Trim(),
                Descripcion = dto.Descripcion,
                Activo = dto.Activo
            };

            await _uow.Roles.Agregar(nuevoRol);
            await _uow.SaveChangesAsync();

            return (true, "Rol creado exitosamente.");
        }

        public async Task<(bool success, string message)> UpdateAsync(RolDto dto)
        {
            var rol = await _uow.Roles.Obtener(dto.Id);

            if (rol == null)
                return (false, "Rol no encontrado.");

            rol.Nombre = dto.Nombre.Trim();
            rol.Descripcion = dto.Descripcion;
            rol.Activo = dto.Activo;

            _uow.Roles.Actualizar(rol);
            await _uow.SaveChangesAsync();

            return (true, "Rol actualizado exitosamente.");
        }

        public async Task<(bool success, string message)> AsignarPermisoAsync(
            int rolId, int permisoId)
        {
            var rol = await _uow.Roles.GetWithPermisosAsync(rolId);

            if (rol == null)
                return (false, "Rol no encontrado.");

            if (rol.RolPermisos.Any(rp => rp.PermisoId == permisoId))
            {
                return (false, "El rol ya tiene ese permiso.");
            }

            var rolPermiso = new RolPermiso
            {
                RolId = rolId,
                PermisoId = permisoId
            };

            _uow.GetContext().Set<RolPermiso>().Add(rolPermiso);
            await _uow.SaveChangesAsync();

            return (true, "Permiso asignado exitosamente.");
        }

        public async Task<(bool success, string message)> RemoverPermisoAsync(
            int rolId, int permisoId)
        {
            var rol = await _uow.Roles.GetWithPermisosAsync(rolId);

            if (rol == null)
                return (false, "Rol no encontrado.");

            var rolPermiso = rol.RolPermisos
                .FirstOrDefault(rp => rp.PermisoId == permisoId);

            if (rolPermiso == null)
            {
                return (false, "El rol no tiene ese permiso.");
            }

            _uow.GetContext().Set<RolPermiso>().Remove(rolPermiso);
            await _uow.SaveChangesAsync();

            return (true, "Permiso removido exitosamente.");
        }

        public async Task<IEnumerable<PermisoDto>> GetAllPermisosAsync()
        {
            var permisos = await _uow.Permisos.ObtenerTodos();
            return permisos.Select(p => new PermisoDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Categoria = p.Categoria
            });
        }

        private RolDto MapToDto(Rol r)
        {
            return new RolDto
            {
                Id = r.Id,
                Nombre = r.Nombre,
                Descripcion = r.Descripcion,
                Activo = r.Activo,
                PermisosIds = r.RolPermisos.Select(rp => rp.PermisoId).ToList(),
                PermisosNombres = r.RolPermisos
                    .Where(rp => rp.Permiso != null)
                    .Select(rp => rp.Permiso!.Nombre)
                    .ToList()
            };
        }
    }
}
