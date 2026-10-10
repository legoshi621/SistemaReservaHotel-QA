using SistemaReservaHotel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaReservaHotel.DataAccess
{

        public interface IPermisoRepository : IRepository<Permiso>
        { 
            Task<IEnumerable<Permiso>> GetByCategoriaAsync(string categoria);
            Task<Permiso?> GetByNombreAsync(string nombre);
        }
    
}
