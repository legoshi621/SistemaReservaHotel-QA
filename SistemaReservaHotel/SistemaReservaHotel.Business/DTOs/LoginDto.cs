using System.ComponentModel.DataAnnotations;

namespace SistemaReservaHotel.Business.DTOs
{        public class LoginDto
        {
            [Required(ErrorMessage = "El email es obligatorio")]
            [EmailAddress(ErrorMessage = "Email inválido")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "La contraseña es obligatoria")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            public bool RememberMe { get; set; }
        }
}