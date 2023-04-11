using AutoMapper;

namespace SGU.API.Modules
{
    public class Usuario_Mapper : Profile
    {
        public Usuario_Mapper()
        {
            CreateMap<Usuario_Entidad, Usuario_Dto>()
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Correo, opt => opt.MapFrom(src => src.Email))
                .ReverseMap();
        }
    }
}
