using Microsoft.EntityFrameworkCore;
using SistemaReservaHotel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaReservaHotel.DataAccess
{
    public class RolRepository : Repository<Rol>, IRolRepository
    {
        public RolRepository(AppDbContext context) : base(context) { }

        public async Task<Rol?> GetByNombreAsync(string nombre)
        {
            return await dbSet
                .Include(r => r.RolPermisos)
                    .ThenInclude(rp => rp.Permiso)
                .FirstOrDefaultAsync(r => r.Nombre == nombre);
        }

        public async Task<Rol?> GetWithPermisosAsync(int id)
        {
            return await dbSet
                .Include(r => r.RolPermisos)
                    .ThenInclude(rp => rp.Permiso)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Rol>> GetRolesActivosAsync()
        {
            return await dbSet
                .Include(r => r.RolPermisos)
                    .ThenInclude(rp => rp.Permiso)
                .Where(r => r.Activo)
                .OrderBy(r => r.Nombre)
                .ToListAsync();
        }
    }
}
