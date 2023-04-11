using Microsoft.AspNetCore.Identity;

namespace SGU.API.Extensions
{
    public static class IdentityExtension
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<Usuario_Entidad, Rol_Entidad>(opts => {
                opts.Password.RequireDigit = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequiredLength = 6;
                opts.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<CentralContext>().AddDefaultTokenProviders();
        }
    }
}
