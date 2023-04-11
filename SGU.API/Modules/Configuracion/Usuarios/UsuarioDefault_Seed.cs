using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SGU.API.Modules
{
    public static class UsuarioDefault_Seed
    {
        public static void AgregarUsuarioDefault(this ModelBuilder builder)
        {
            string idRol = Guid.NewGuid().ToString();
            string idUsuario = Guid.NewGuid().ToString();

            // Agregar el rol
            builder.Entity<Rol_Entidad>().HasData(new Rol_Entidad
            {
                Id = idRol,
                Name = "Super Admin",
                NormalizedName = "SUPER ADMIN",
                ConcurrencyStamp = idRol
            });

            //Crear el usuario
            var usuario = new Usuario_Entidad
            {
                Id = idUsuario,
                UserName = "superadmin",
                NormalizedUserName = "SUPERADMIN",
                Nombre = "Henry",
                Apellido = "Pimentel",
                Email = "superadmin@superadmin.com",
                EmailConfirmed = true,
                Estado = true
            };

            //Crear la contraseña
            PasswordHasher<Usuario_Entidad> ph = new PasswordHasher<Usuario_Entidad>();
            usuario.PasswordHash = ph.HashPassword(usuario, "@Solidweb137*");

            //Agregar usuario
            builder.Entity<Usuario_Entidad>().HasData(usuario);

            //Agregar rol al usuario
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = idRol,
                UserId = idUsuario,
            });

            //Agrega claims a usuarios
            var claims = new List<IdentityUserClaim<string>>
            {
                new IdentityUserClaim<string>
                {
                    Id = 1,
                    UserId = idUsuario,
                    ClaimType = ClaimTypes.NameIdentifier,
                    ClaimValue = idUsuario
                },
                new IdentityUserClaim<string>
                {
                    Id = 2,
                    UserId = idUsuario,
                    ClaimType = ClaimTypes.Role,
                    ClaimValue= "Super Admin"
                },
                new IdentityUserClaim<string>
                {
                    Id = 3,
                    UserId = idUsuario,
                    ClaimType = "Nombre",
                    ClaimValue =$"{usuario.Nombre} {usuario.Apellido}"
                }
            };

            foreach (var claim in claims)
            {
                builder.Entity<IdentityUserClaim<string>>().HasData(claim);
            }
        }
    }
}
