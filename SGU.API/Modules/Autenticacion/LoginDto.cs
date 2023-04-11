using System.ComponentModel.DataAnnotations;

namespace SGU.API.Modules
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El nombre usuario es obligatorio")]
        public string? NombreUsuario { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string? Contrasenia { get; set; }
    }
}
