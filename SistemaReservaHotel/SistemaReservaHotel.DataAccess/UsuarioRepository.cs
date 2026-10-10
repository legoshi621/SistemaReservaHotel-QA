using Microsoft.EntityFrameworkCore;
using SistemaReservaHotel.Entities;

namespace SistemaReservaHotel.DataAccess
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AppDbContext context) : base(context) { }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await dbSet
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario?> GetWithRolesAsync(int id)
        {
            return await dbSet
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                        .ThenInclude(r => r.RolPermisos)
                            .ThenInclude(rp => rp.Permiso)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosActivosAsync()
        {
            return await dbSet
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .Where(u => u.Activo)
                .OrderBy(u => u.NombreCompleto)
                .ToListAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await dbSet.AnyAsync(u => u.Email == email);
        }
    }
}