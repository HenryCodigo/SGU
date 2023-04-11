using System.ComponentModel.DataAnnotations;

namespace SGU.API.Modules
{
    public class CambioContraseniaDto
    {
        [Required(ErrorMessage = "La contraseña actual es obligatoria.")]
        [DataType(DataType.Password)]
        public string ContraseniaActual { get; set; }
        [Required(ErrorMessage = "La contraseña nueva es obligatoria.")]
        [DataType(DataType.Password)]
        public string NuevaContrasenia { get; set; }
        [Required(ErrorMessage = "La confirmación de la contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string ConfirmacionContrasenia { get; set; }
    }
}
