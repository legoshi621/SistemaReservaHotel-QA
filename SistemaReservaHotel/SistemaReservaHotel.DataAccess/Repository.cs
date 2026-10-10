using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SistemaReservaHotel.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected DbSet<T> dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;

            this.dbSet = _context.Set<T>();
        }

        public async Task<T?> Obtener(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> ObtenerTodos()
        {
            return await dbSet.ToListAsync();
        }

        public async Task Agregar(T entidad)
        {
            await dbSet.AddAsync(entidad);
        }

        public void Actualizar(T entidad)
        {
            dbSet.Update(entidad);
        }

        public void Eliminar(T entidad)
        {
            dbSet.Remove(entidad);
        }

        public async Task<bool> GuardarCambios()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}