using System.Security.Claims;

namespace SistemaReservaHotel.WebMVC.Helpers
{
    public static class ClaimsExtensions
    {
        public static bool TienePermiso(this ClaimsPrincipal user, string permiso)
        {
            return user.HasClaim("Permiso", permiso);
        }

        public static bool TieneCualquierPermiso(this ClaimsPrincipal user, params string[] permisos)
        {
            return permisos.Any(p => user.HasClaim("Permiso", p));
        }

        public static bool TieneTodosLosPermisos(this ClaimsPrincipal user, params string[] permisos)
        {
            return permisos.All(p => user.HasClaim("Permiso", p));
        }

        public static int GetUsuarioId(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }

        public static string GetEmail(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        }
    }
}
