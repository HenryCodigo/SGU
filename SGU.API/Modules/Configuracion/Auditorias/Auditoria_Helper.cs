using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace SGU.API.Modules
{
    public class Auditoria_Helper
    {
        public Auditoria_Helper(EntityEntry entry) => Entry = entry;

        public EntityEntry Entry { get; }
        public string UsuarioId { get; set; }
        public string NombreTabla { get; set; }
        public Dictionary<string, object> ClaveValor { get; } = new();
        public Dictionary<string, object> ValorAnterior { get; } = new();
        public Dictionary<string, object> NuevoValor { get; } = new();
        public TipoAuditoria_Enum TipoAuditoria { get; set; }
        public List<string> ColumnasCambiadas { get; } = new();

        public Auditoria_Entidad ConvertirEnAuditoria()
        {
            var auditoria = new Auditoria_Entidad();
            auditoria.UsuarioId = UsuarioId;
            auditoria.NombreTabla = NombreTabla;
            auditoria.Tipo = TipoAuditoria.ToString();
            auditoria.Fecha = DateTime.Now;
            auditoria.ClavePrimaria = JsonConvert.SerializeObject(ClaveValor);
            auditoria.ValorAnterior = ValorAnterior.Count == 0 ? null : JsonConvert.SerializeObject(ValorAnterior);
            auditoria.NuevoValor = NuevoValor.Count == 0 ? null : JsonConvert.SerializeObject(NuevoValor);
            auditoria.ColumnaAfectada = ColumnasCambiadas.Count == 0 ? null : JsonConvert.SerializeObject(ColumnasCambiadas);

            return auditoria;
        }
    }
}
