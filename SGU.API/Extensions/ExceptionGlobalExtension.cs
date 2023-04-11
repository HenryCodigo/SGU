using SGU.API.Exceptions;

namespace SGU.API.Extensions
{
    public static class ExceptionGlobalExtension
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
            => app.UseMiddleware<ExceptionMiddleware>();
    }
}
