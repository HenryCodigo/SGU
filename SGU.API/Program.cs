using SGU.API.Extensions;
using System.Reflection;

namespace SGU.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigurationManager configuration = builder.Configuration;

            //Agrega Cors
            builder.Services.AddCors();

            // Agregar Entity Framework Core
            builder.Services.ConfigureEntity(configuration);

            // Agregar Identity Framework Core
            builder.Services.ConfigureIdentity();

            // Agregar Authentication y Jwt
            builder.Services.ConfigureAuthentication(configuration);

            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            
            //Agregar Swagger
            builder.Services.ConfigureSwagger();

            // Agregar AutoMapper
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Agregar las disntitas interfaces de la aplicacion
            builder.Services.AddInterfaces();

            var app = builder.Build();

            app.UseGlobalExceptionHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(op =>
                {
                    op.SwaggerEndpoint("/swagger/V1/swagger.json", "Proyecto Base Api");
                });
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyMethod()
                );

            app.Run();
        }
    }
}