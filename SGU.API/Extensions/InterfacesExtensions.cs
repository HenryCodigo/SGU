namespace SGU.API.Extensions
{
    public static class InterfacesExtensions
    {
        public static void AddInterfaces(this IServiceCollection services)
        {
            services.AddScoped<Usuario_Interface, Usuario_Service>();
        }
    }
}
