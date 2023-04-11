using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SGU.API.Exceptions;

namespace SGU.API.Modules
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly Usuario_Interface _usuario;
        public UsuariosController(Usuario_Interface usuario)
        {
            _usuario = usuario;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            var lista = await _usuario.ListaUsuarios();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        [Route("buscar-por-id")]
        public async Task<IActionResult> BuscarPorId(string id)
        {
            var usuario = await _usuario.BuscarUsuarioId(id);
            
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(Usuario_Dto usuarioDto, string rol)
        {
            var usuario = await _usuario.Agregar(usuarioDto, rol);
            
            return Created($"/buscar-por-id?id={usuario.Id}", usuario);
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar(Usuario_Dto usuarioDto, string rol)
        {
            var usuario = await _usuario.Actualizar(usuarioDto, rol);
            
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(string idUsuario)
        {
            var seElimino = await _usuario.Eliminar(idUsuario);
            
            return seElimino ? NoContent() : NotFound();
        }

        [HttpPost]
        [Route("reiniciar-contrasenia")]
        public async Task<IActionResult> ReiniciarContrasenia(string idUsuario)
        {
            var resultado = await _usuario.ReiniciarContrasenia(idUsuario);

            return resultado ? Ok() : BadRequest();
        }

        [HttpPost]
        [Route("cambiar-contrasenia")]
        public async Task<IActionResult> CambiarContrasenia(Usuario_Dto usuarioDto,
            CambioContraseniaDto cambioContrasenia)
        {
            var resultado = await _usuario.CambiarContrasenia(usuarioDto, cambioContrasenia);

            return resultado ? Ok() : BadRequest();
        }
    }
}
