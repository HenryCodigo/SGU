using System.ComponentModel.DataAnnotations;

namespace SGU.API.Modules
{
    public class Usuario_Dto
    {
        public string? Id { get; set; }
        [Required(ErrorMessage = "El nombre usuario es obligatorio")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "El correo es obligatorio")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Contrasenia { get; set; }
        public bool Estado { get; set; }
    }
}
