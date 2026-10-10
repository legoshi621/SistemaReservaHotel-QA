using SistemaReservaHotel.DataAccess;
using System;
using System.Collections.Generic;

namespace SistemaReservaHotel.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {

        // ═══ NUEVOS REPOSITORIOS DE SEGURIDAD ═══
        IUsuarioRepository Usuarios { get; }
        IRolRepository Roles { get; }
        IPermisoRepository Permisos { get; }

        Task<int> SaveChangesAsync();
    }
}
