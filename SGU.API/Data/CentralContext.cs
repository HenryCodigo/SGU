using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SGU.API.Modules;

namespace SGU.API.Data
{
    public class CentralContext : IdentityDbContext<Usuario_Entidad>
    {
        public IHttpContextAccessor _httpContext { get; }
        public CentralContext(DbContextOptions options, IHttpContextAccessor httpContext)
            : base(options)
        {
            _httpContext = httpContext;
        }

        #region Sets Configuracion
        public DbSet<Auditoria_Entidad> AuditoriaSet { get; set; }
        public DbSet<Usuario_Entidad> UsuarioSet { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.AgregarUsuarioDefault();

            base.OnModelCreating(builder);
        }

        #region Auditoria
        public virtual int SaveChanges(string idUsuario)
        {
            AuditarGuardar(idUsuario);
            return base.SaveChanges();
        }

        public virtual async Task<int> SaveChangesAsync(string idUsuario)
        {
            AuditarGuardar(idUsuario);
            return await base.SaveChangesAsync();
        }

        private void AuditarGuardar(string idUsuario)
        {
            try
            {
                //var login = _httpContext?.HttpContext?.User?.FindFirst(ClaimTypes.Name).Value;
                ChangeTracker.DetectChanges();
                var listaAuditoria = new List<Auditoria_Helper>();

                foreach (var entry in ChangeTracker.Entries())
                {
                    if (entry.Entity is EntidadBase auditable)
                        auditable.ActualizarFecha(entry.State);

                    if (entry.Entity is Auditoria_Entidad ||
                        entry.State is EntityState.Detached or EntityState.Unchanged)
                        continue;

                    var auditoriaHelpers = new Auditoria_Helper(entry)
                    {
                        NombreTabla = entry.Entity.GetType().Name,
                        //UsuarioId = login != null ? idUsuario.ToString() : "Sistema_ID",
                        UsuarioId = idUsuario != null ? idUsuario.ToString() : "Sistema_ID",
                    };

                    listaAuditoria.Add(auditoriaHelpers);

                    foreach (var propiedad in entry.Properties)
                    {
                        var nombrePropiedad = propiedad.Metadata.Name;

                        if (propiedad.Metadata.IsPrimaryKey())
                        {
                            auditoriaHelpers.ClaveValor[nombrePropiedad] = propiedad.CurrentValue;
                            continue;
                        }

                        switch (entry.State)
                        {
                            case EntityState.Deleted:
                                auditoriaHelpers.TipoAuditoria = TipoAuditoria_Enum.Eliminar;
                                auditoriaHelpers.ValorAnterior[nombrePropiedad] = propiedad.OriginalValue;
                                break;
                            case EntityState.Modified:
                                if (propiedad.IsModified)
                                {
                                    auditoriaHelpers.ColumnasCambiadas.Add(nombrePropiedad);
                                    auditoriaHelpers.TipoAuditoria = TipoAuditoria_Enum.Edicion;
                                    auditoriaHelpers.ValorAnterior[nombrePropiedad] = propiedad.OriginalValue;
                                    auditoriaHelpers.NuevoValor[nombrePropiedad] = propiedad.CurrentValue;
                                }
                                break;
                            case EntityState.Added:
                                auditoriaHelpers.TipoAuditoria = TipoAuditoria_Enum.Creacion;
                                auditoriaHelpers.NuevoValor[nombrePropiedad] = propiedad.CurrentValue;
                                break;
                        }

                        //listaTemporal.Add(auditoriaHelpers.ConvertirToAuditoria());
                    }

                }

                foreach (var elemt in listaAuditoria)
                {
                    AuditoriaSet.Add(elemt.ConvertirEnAuditoria());
                }

                //await AuditoriaSet.AddRangeAsync(listaTemporal);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        #endregion
    }
}
