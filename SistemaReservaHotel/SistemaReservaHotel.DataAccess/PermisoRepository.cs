using Microsoft.EntityFrameworkCore;
using SistemaReservaHotel.DataAccess;
using SistemaReservaHotel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaReservaHotel.DataAccess
{
    public class PermisoRepository : Repository<Permiso>, IPermisoRepository
    { 

        public PermisoRepository(AppDbContext context) : base(context) {
        }


        public async Task<IEnumerable<Permiso>> GetByCategoriaAsync(string categoria)
        {


            return await dbSet
                .Where(p => p.Categoria == categoria)
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<Permiso?> GetByNombreAsync(string nombre)
        {
            return await dbSet
                .FirstOrDefaultAsync(p => p.Nombre == nombre);
        }
    }
}
