using System.Linq.Expressions;

namespace SistemaReservaHotel.DataAccess
{
    public interface IRepository<T> where T : class
    {
        Task<T?> Obtener(int id);
        Task<IEnumerable<T>> ObtenerTodos();
        Task Agregar(T entidad);
        void Actualizar(T entidad);
        void Eliminar(T entidad);
        Task<bool> GuardarCambios();
    }
}
