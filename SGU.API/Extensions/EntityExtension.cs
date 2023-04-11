using Microsoft.EntityFrameworkCore;

namespace SGU.API.Extensions
{
    public static class EntityExtension
    {
        public static void ConfigureEntity(this IServiceCollection services,
            ConfigurationManager configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<CentralContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .EnableSensitiveDataLogging();
            });
        }
    }
}
