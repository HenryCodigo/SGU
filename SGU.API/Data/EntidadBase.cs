using Microsoft.EntityFrameworkCore;

namespace SGU.API.Data
{
    public class EntidadBase
    {
        public virtual DateTime? Creado { get; set; }
        public virtual DateTime? Editado { get; set; }
        public virtual DateTime? Eliminado { get; set; }

        public void ActualizarFecha(EntityState state)
        {
            switch (state)
            {
                case EntityState.Deleted:
                    Eliminado = DateTime.Now;
                    break;
                case EntityState.Modified:
                    Editado = DateTime.Now;
                    break;
                case EntityState.Added:
                    Creado = DateTime.Now;
                    break;
                default:
                    break;
            }
        }
    }
}
