using SistemaReservaHotel.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaReservaHotel.Business.Services
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioDto>> GetAllAsync();
        Task<UsuarioDto?> GetByIdAsync(int id);
        Task<(bool success, string message)> CreateAsync(UsuarioDto dto, string password);
        Task<(bool success, string message)> UpdateAsync(UsuarioDto dto);
        Task<(bool success, string message)> DeleteAsync(int id);
        Task<(bool success, string message)> AsignarRolAsync(int usuarioId, int rolId);
        Task<(bool success, string message)> RemoverRolAsync(int usuarioId, int rolId);
        Task<(bool success, string message)> CambiarPasswordAsync(int usuarioId, string nuevaPassword);
    }
}
