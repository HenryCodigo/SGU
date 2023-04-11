using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SGU.API.Exceptions;
using System.Security.Claims;

namespace SGU.API.Modules
{
    public class Usuario_Service : Usuario_Interface
    {
        private readonly UserManager<Usuario_Entidad> _userManager;
        private readonly IMapper _mapper;
        public Usuario_Service(UserManager<Usuario_Entidad> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<Usuario_Dto>> ListaUsuarios()
        {
            var db_usuarios = await _userManager.Users
                .AsNoTracking()
                .OrderBy(u => u.UserName)
                .ToListAsync();

            if (db_usuarios.Count == 0)
                throw new NotFoundException($"No hay usuarios en la base de datos.");

            var resultado = _mapper.Map<List<Usuario_Dto>>(db_usuarios);

            return resultado;
        }

        public async Task<Usuario_Dto> BuscarUsuarioId(string idUsuario)
        {
            var db_usuario = await _userManager.FindByIdAsync(idUsuario) ??
                throw new NotFoundException($"No se encontro el usuario ID: {idUsuario}");
            
            var resultado = _mapper.Map<Usuario_Dto>(db_usuario);

            return resultado;
        }

        public async Task<Usuario_Dto> BuscarUsuarioNombre(string nombreUsuario)
        {
            var db_usuario = await _userManager.FindByNameAsync(nombreUsuario) ??
                throw new NotFoundException($"No se encontro el usuario: {nombreUsuario}");

            var resultado = _mapper.Map<Usuario_Dto>(db_usuario);
            
            return resultado;
        }

        public async Task<Usuario_Dto> Agregar(Usuario_Dto usuarioDto, string rol)
        {
            usuarioDto.Id = Guid.NewGuid().ToString();
            usuarioDto.Estado = true;

            var usuario = _mapper.Map<Usuario_Entidad>(usuarioDto);

            var usuarioCreado = await _userManager.CreateAsync(usuario, usuarioDto.Contrasenia);

            if (usuarioCreado.Succeeded)
            {
                var listaClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim("NombreCompleto", $"{usuario.Nombre} {usuario.Apellido}"),
                    new Claim(ClaimTypes.Role, rol)
                };

                var claimsCreados = await _userManager.AddClaimsAsync(usuario, listaClaims);

                return claimsCreados.Succeeded ? usuarioDto : null;
            }
            else
                return null;
        }

        public async Task<Usuario_Dto> Actualizar(Usuario_Dto usuarioDto, string rol)
        {
            var db_usuario = await _userManager.FindByIdAsync(usuarioDto.Id);

            _mapper.Map(usuarioDto, db_usuario);

            var todosClaims = await _userManager.GetClaimsAsync(db_usuario);
            await _userManager.RemoveClaimsAsync(db_usuario, todosClaims);

            await ActualizarClaim(db_usuario, ClaimTypes.NameIdentifier, db_usuario.Id);
            await ActualizarClaim(db_usuario, "NombreCompleto", $"{db_usuario.Nombre} {db_usuario.Apellido}");
            await ActualizarClaim(db_usuario, ClaimTypes.Role, rol);

            var usuarioActulizado = await _userManager.UpdateAsync(db_usuario);

            return usuarioActulizado.Succeeded ? usuarioDto : null;
        }

        public async Task<bool> Eliminar(string idUsuario)
        {
            var db_usuario = await _userManager.FindByIdAsync(idUsuario);

            var usuarioEliminado = await _userManager.DeleteAsync(db_usuario);
            
            return usuarioEliminado.Succeeded ? true : false;
        }

        public async Task<bool> ReiniciarContrasenia(string idUsuario)
        {
            var usuarioDto = await BuscarUsuarioId(idUsuario);
            var db_usuario = _mapper.Map<Usuario_Entidad>(usuarioDto);

            var tokenReinicio = await _userManager.GeneratePasswordResetTokenAsync(db_usuario);
            var reinicio = await _userManager.ResetPasswordAsync(db_usuario, tokenReinicio, db_usuario.UserName);

            return reinicio.Succeeded ? true : false;
        }

        public async Task<bool> CambiarContrasenia(Usuario_Dto usuarioDto, CambioContraseniaDto cambioContrasenia)
        {
            if (usuarioDto == null || cambioContrasenia == null)
                return false;

            var usuario = _mapper.Map<Usuario_Entidad>(usuarioDto);

            var resultado = await _userManager.ChangePasswordAsync(usuario, cambioContrasenia.ContraseniaActual,
                cambioContrasenia.NuevaContrasenia);

            return resultado.Succeeded ? true : false;
        }

        public async Task<Usuario_Entidad> UsuarioAutentificado(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }

        private async Task<IdentityResult> ActualizarClaim(Usuario_Entidad usuario, string nombreClaim, string valorClaim)
        {
            var listaClaims = await _userManager.GetClaimsAsync(usuario);

            var claim = listaClaims.Where(x => x.Type == nombreClaim).FirstOrDefault();

            if (claim != null)
                await _userManager.RemoveClaimAsync(usuario, claim);

            var claimNuevo = new Claim(nombreClaim, valorClaim);

            return await _userManager.AddClaimAsync(usuario, claimNuevo);
        }
    }
}
