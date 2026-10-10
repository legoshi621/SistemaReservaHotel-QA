using SistemaReservaHotel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaReservaHotel.DataAccess
{
    public interface IRolRepository : IRepository<Rol>
    {
        Task<Rol?> GetByNombreAsync(string nombre);
        Task<Rol?> GetWithPermisosAsync(int id);
        Task<IEnumerable<Rol>> GetRolesActivosAsync();
    }
}
