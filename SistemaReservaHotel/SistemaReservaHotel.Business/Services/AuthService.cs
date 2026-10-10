using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SistemaReservaHotel.Business.DTOs;
using SistemaReservaHotel.DataAccess.UnitOfWork;
using SistemaReservaHotel.Entities;

namespace SistemaReservaHotel.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;

        public AuthService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<(bool success, string message, UsuarioDto? usuario)>
            LoginAsync(LoginDto dto)
        {
            // Buscar usuario por email
            var usuario = await _uow.Usuarios.GetByEmailAsync(dto.Email);

            if (usuario == null || !usuario.Activo)
            {
                return (false, "Email o contraseña incorrectos.", null);
            }

            // Validar contraseña
            if (!VerifyPasswordHash(dto.Password, usuario.PasswordHash))
            {
                return (false, "Email o contraseña incorrectos.", null);
            }

            // Actualizar último acceso
            usuario.UltimoAcceso = DateTime.Now;
            _uow.Usuarios.Actualizar(usuario);
            await _uow.SaveChangesAsync();

            // Convertir a DTO
            var usuarioDto = MapToDto(usuario);

            return (true, "Login exitoso.", usuarioDto);
        }
        public async Task<(bool success, string message)>
           RegisterAsync(RegisterDto dto, string rolPorDefecto = "Usuario")
        {
            // Verificar si el email ya existe
            if (await _uow.Usuarios.EmailExistsAsync(dto.Email))
            {
                return (false, $"El email {dto.Email} ya está registrado.");
            }

            // Crear nuevo usuario
            var nuevoUsuario = new Usuario
            {
                NombreCompleto = dto.NombreCompleto.Trim(),
                Email = dto.Email.Trim().ToLower(),
                PasswordHash = HashPassword(dto.Password),
                Telefono = dto.Telefono,
                Activo = true,
                FechaCreacion = DateTime.Now
            };

            await _uow.Usuarios.Agregar(nuevoUsuario);
            await _uow.SaveChangesAsync();

            // Asignar rol por defecto
            var rol = await _uow.Roles.GetByNombreAsync(rolPorDefecto);

            if (rol != null)
            {
                var usuarioRol = new UsuarioRol
                {
                    UsuarioId = nuevoUsuario.Id,
                    RolId = rol.Id,
                    FechaAsignacion = DateTime.Now
                };

                // Nota: Necesitarás un repositorio para UsuarioRol
                // o agregar directamente al contexto
                _uow.GetContext().Set<UsuarioRol>().Add(usuarioRol);
                await _uow.SaveChangesAsync();
            }

            return (true, "Usuario registrado exitosamente.");
        }
        public async Task<bool> ValidarPasswordAsync(string email, string password)
        {
            var usuario = await _uow.Usuarios.GetByEmailAsync(email);

            if (usuario == null || !usuario.Activo)
                return false;

            return VerifyPasswordHash(password, usuario.PasswordHash);
        }

        public async Task<UsuarioDto?> GetUsuarioByEmailAsync(string email)
        {
            var usuario = await _uow.Usuarios.GetByEmailAsync(email);

            return usuario == null ? null : MapToDto(usuario);
        }

        public async Task<List<string>> GetRolesUsuarioAsync(int usuarioId)
        {
            var usuario = await _uow.Usuarios.GetWithRolesAsync(usuarioId);

            if (usuario == null)
                return new List<string>();

            return usuario.UsuarioRoles
                .Where(ur => ur.Rol != null)
                .Select(ur => ur.Rol!.Nombre)
                .ToList();
        }

        public async Task<List<string>> GetPermisosUsuarioAsync(int usuarioId)
        {
            var usuario = await _uow.Usuarios.GetWithRolesAsync(usuarioId);

            if (usuario == null)
                return new List<string>();

            // Obtener todos los permisos de todos los roles del usuario
            var permisos = usuario.UsuarioRoles
                .Where(ur => ur.Rol != null)
                .SelectMany(ur => ur.Rol!.RolPermisos)
                .Where(rp => rp.Permiso != null)
                .Select(rp => rp.Permiso!.Nombre)
                .Distinct()
                .ToList();

            return permisos;
        }
        // ═══ MÉTODOS PRIVADOS ═══

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }

        private UsuarioDto MapToDto(Usuario usuario)
        {
            return new UsuarioDto
            {
                Id = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                Email = usuario.Email,
                Telefono = usuario.Telefono,
                Activo = usuario.Activo,
                FechaCreacion = usuario.FechaCreacion,
                UltimoAcceso = usuario.UltimoAcceso,
                Roles = usuario.UsuarioRoles
                    .Where(ur => ur.Rol != null)
                    .Select(ur => ur.Rol!.Nombre)
                    .ToList()
            };
        }
    }

    // Extensión para obtener DbContext desde UnitOfWork
    public static class UnitOfWorkExtensions
    {
        public static DbContext GetContext(this IUnitOfWork uow)
        {
            
            return ((UnitOfWork)uow).Context;
        }
    }
}
