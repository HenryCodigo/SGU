using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGU.API.Modules
{
    [Table("Auditoria")]
    public class Auditoria_Entidad
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string UsuarioId { get; set; }
        [MaxLength(100)]
        public string Tipo { get; set; }
        [MaxLength(150)]
        public string NombreTabla { get; set; }
        public DateTime Fecha { get; set; }
        public string? ValorAnterior { get; set; }
        public string? NuevoValor { get; set; }
        public string? ColumnaAfectada { get; set; }
        public string? ClavePrimaria { get; set; }
    }
}
