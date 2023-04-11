using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SGU.API.Modules
{
    public class Usuario_Entidad : IdentityUser
    {
        [Required]
        [StringLength(200)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(200)]
        public string Apellido { get; set; }
        public bool Estado { get; set; }
    }
}
