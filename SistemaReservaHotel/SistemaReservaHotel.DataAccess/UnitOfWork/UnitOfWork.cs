using SistemaReservaHotel.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaReservaHotel.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

      
        private readonly AppDbContext _context;

        // ═══ REPOSITORIOS EXISTENTES ═══

        // ═══ NUEVOS REPOSITORIOS ═══
        public IUsuarioRepository Usuarios { get; private set; }
        public IRolRepository Roles { get; private set; }
        public IPermisoRepository Permisos { get; private set; }
        public AppDbContext Context => _context;


        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            // Inicializar nuevos repositorios
            Usuarios = new UsuarioRepository(context);
            Roles = new RolRepository(context);
            Permisos = new PermisoRepository(context);
        }

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }

}

