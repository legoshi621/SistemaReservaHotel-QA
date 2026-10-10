using SistemaReservaHotel.Business.DTOs;

namespace SistemaReservaHotel.Business.Services
{
    public interface IAuthService
    {
        Task<(bool success, string message, UsuarioDto? usuario)>
            LoginAsync(LoginDto dto);

        Task<(bool success, string message)>
            RegisterAsync(RegisterDto dto, string rolPorDefecto = "Usuario");

        Task<bool> ValidarPasswordAsync(string email, string password);

        Task<UsuarioDto?> GetUsuarioByEmailAsync(string email);

        Task<List<string>> GetRolesUsuarioAsync(int usuarioId);

        Task<List<string>> GetPermisosUsuarioAsync(int usuarioId);
    }
}