using SistemaReservaHotel.DataAccess;
using SistemaReservaHotel.Entities;


namespace SistemaReservaHotel.DataAccess
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task<Usuario?> GetWithRolesAsync(int id);
        Task<IEnumerable<Usuario>> GetUsuariosActivosAsync();
        Task<bool> EmailExistsAsync(string email);
    }
}