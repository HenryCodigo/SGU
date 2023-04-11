namespace SGU.API.Modules
{
    public interface Usuario_Interface
    {
        Task<List<Usuario_Dto>> ListaUsuarios();
        
        Task<Usuario_Dto> BuscarUsuarioId(string idUsuario);
        Task<Usuario_Dto> BuscarUsuarioNombre(string nombreUsuario);
        
        Task<Usuario_Dto> Agregar(Usuario_Dto usuarioDto, string rol);
        Task<Usuario_Dto> Actualizar(Usuario_Dto usuario, string rol);
        Task<bool> Eliminar(string idUsuario);

        Task<bool> ReiniciarContrasenia(string idUsuario);
        Task<bool> CambiarContrasenia(Usuario_Dto usuarioDto, CambioContraseniaDto cambioContrasenia);
    }
}
