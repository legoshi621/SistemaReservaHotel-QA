using SistemaReservaHotel.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaReservaHotel.Business.Services
{
    public interface IRolService
    {
        Task<IEnumerable<RolDto>> GetAllAsync();
        Task<RolDto?> GetByIdAsync(int id);
        Task<(bool success, string message)> CreateAsync(RolDto dto);
        Task<(bool success, string message)> UpdateAsync(RolDto dto);
        Task<(bool success, string message)> AsignarPermisoAsync(int rolId, int permisoId);
        Task<(bool success, string message)> RemoverPermisoAsync(int rolId, int permisoId);
        Task<IEnumerable<PermisoDto>> GetAllPermisosAsync();
    }
}
