using SistemaReservaHotel.Business.DTOs;
using SistemaReservaHotel.Business.Services;
using SistemaReservaHotel.DataAccess; // Donde tienes tu Repositorio
using SistemaReservaHotel.DataAccess.UnitOfWork;
using SistemaReservaHotel.Entities;
using System.Security.Cryptography;
using System.Text;

namespace SistemaReservaHotel.Business.services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _uow;

        public UsuarioService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
        {
            var usuarios = await _uow.Usuarios.GetUsuariosActivosAsync();
            return usuarios.Select(MapToDto);
        }

        public async Task<UsuarioDto?> GetByIdAsync(int id)
        {
            var usuario = await _uow.Usuarios.GetWithRolesAsync(id);
            return usuario == null ? null : MapToDto(usuario);
        }

        public async Task<(bool success, string message)> CreateAsync(
            UsuarioDto dto, string password)
        {
            // Validar que el email no exista
            if (await _uow.Usuarios.EmailExistsAsync(dto.Email))
            {
                return (false, $"El email {dto.Email} ya está registrado.");
            }

            var nuevoUsuario = new Usuario
            {
                NombreCompleto = dto.NombreCompleto.Trim(),
                Email = dto.Email.Trim().ToLower(),
                PasswordHash = HashPassword(password),
                Telefono = dto.Telefono,
                Activo = dto.Activo,
                FechaCreacion = DateTime.Now
            };

            await _uow.Usuarios.Agregar(nuevoUsuario);
            await _uow.SaveChangesAsync();

            return (true, "Usuario creado exitosamente.");
        }

        public async Task<(bool success, string message)> UpdateAsync(UsuarioDto dto)
        {
            var usuario = await _uow.Usuarios.Obtener(dto.Id);

            if (usuario == null)
                return (false, "Usuario no encontrado.");

            // Verificar si el email cambió y ya existe
            if (usuario.Email != dto.Email.ToLower())
            {
                if (await _uow.Usuarios.EmailExistsAsync(dto.Email))
                {
                    return (false, $"El email {dto.Email} ya está en uso.");
                }
            }

            usuario.NombreCompleto = dto.NombreCompleto.Trim();
            usuario.Email = dto.Email.Trim().ToLower();
            usuario.Telefono = dto.Telefono;
            usuario.Activo = dto.Activo;

            _uow.Usuarios.Actualizar(usuario);
            await _uow.SaveChangesAsync();

            return (true, "Usuario actualizado exitosamente.");
        }

        public async Task<(bool success, string message)> DeleteAsync(int id)
        {
            var usuario = await _uow.Usuarios.Obtener(id);

            if (usuario == null)
                return (false, "Usuario no encontrado.");

            // Soft delete
            usuario.Activo = false;
            _uow.Usuarios.Actualizar(usuario);
            await _uow.SaveChangesAsync();

            return (true, "Usuario desactivado exitosamente.");
        }

        public async Task<(bool success, string message)> AsignarRolAsync(
            int usuarioId, int rolId)
        {
            var usuario = await _uow.Usuarios.GetWithRolesAsync(usuarioId);

            if (usuario == null)
                return (false, "Usuario no encontrado.");

            var rol = await _uow.Roles.Obtener(rolId);

            if (rol == null)
                return (false, "Rol no encontrado.");

            // Verificar si ya tiene el rol
            if (usuario.UsuarioRoles.Any(ur => ur.RolId == rolId))
            {
                return (false, $"El usuario ya tiene el rol {rol.Nombre}.");
            }

            var usuarioRol = new UsuarioRol
            {
                UsuarioId = usuarioId,
                RolId = rolId,
                FechaAsignacion = DateTime.Now
            };

            _uow.GetContext().Set<UsuarioRol>().Add(usuarioRol);
            await _uow.SaveChangesAsync();

            return (true, $"Rol {rol.Nombre} asignado exitosamente.");
        }

        public async Task<(bool success, string message)> RemoverRolAsync(
            int usuarioId, int rolId)
        {
            var usuario = await _uow.Usuarios.GetWithRolesAsync(usuarioId);

            if (usuario == null)
                return (false, "Usuario no encontrado.");

            var usuarioRol = usuario.UsuarioRoles
                .FirstOrDefault(ur => ur.RolId == rolId);

            if (usuarioRol == null)
            {
                return (false, "El usuario no tiene ese rol asignado.");
            }

            _uow.GetContext().Set<UsuarioRol>().Remove(usuarioRol);
            await _uow.SaveChangesAsync();

            return (true, "Rol removido exitosamente.");
        }

        public async Task<(bool success, string message)> CambiarPasswordAsync(
            int usuarioId, string nuevaPassword)
        {
            var usuario = await _uow.Usuarios.Obtener(usuarioId);

            if (usuario == null)
                return (false, "Usuario no encontrado.");

            usuario.PasswordHash = HashPassword(nuevaPassword);
            _uow.Usuarios.Actualizar(usuario);
            await _uow.SaveChangesAsync();

            return (true, "Contraseña actualizada exitosamente.");
        }

        // ═══ MÉTODOS PRIVADOS ═══

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private UsuarioDto MapToDto(Usuario u)
        {
            return new UsuarioDto
            {
                Id = u.Id,
                NombreCompleto = u.NombreCompleto,
                Email = u.Email,
                Telefono = u.Telefono,
                Activo = u.Activo,
                FechaCreacion = u.FechaCreacion,
                UltimoAcceso = u.UltimoAcceso,
                Roles = u.UsuarioRoles
                    .Where(ur => ur.Rol != null)
                    .Select(ur => ur.Rol!.Nombre)
                    .ToList()
            };
        }
    }
}

