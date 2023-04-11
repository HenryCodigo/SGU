using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SGU.API.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UnauthorizedAccessException = SGU.API.Exceptions.UnauthorizedAccessException;

namespace SGU.API.Modules
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly UserManager<Usuario_Entidad> _userManager;
        private readonly RoleManager<Rol_Entidad> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<Usuario_Entidad> _signInManager;
        public AutenticacionController(UserManager<Usuario_Entidad> userManager,
            RoleManager<Rol_Entidad> roleManager, IConfiguration configuration, 
            SignInManager<Usuario_Entidad> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            if (login is null)
                throw new BadRequestException($"El usuario o la contraseña son incorrecto.");

            var db_usuario = await _userManager.FindByNameAsync(login.NombreUsuario);

            if (db_usuario != null)
            {
                await _signInManager.SignOutAsync();
                var resultado = await _signInManager.PasswordSignInAsync(db_usuario,
                    login.Contrasenia, true, true);

                if (resultado.Succeeded)
                {
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, db_usuario.Id),
                        new Claim("NombreCompleto", $"{db_usuario.Nombre} {db_usuario.Apellido}"),
                        new Claim(ClaimTypes.Role, "Normal"),
                    };

                    var token = ObtenerToken(authClaims);

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
                else
                    throw new UnauthorizedAccessException($"El usuario o la contraseña son incorrecto");
            }
            else
                throw new UnauthorizedAccessException($"El usuario o la contraseña son incorrecto");
        }

        private JwtSecurityToken ObtenerToken(List<Claim> listaClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding
                            .UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: listaClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
