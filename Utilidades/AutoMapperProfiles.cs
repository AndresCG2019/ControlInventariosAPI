using AutoMapper;
using ControlInventariosAPI.DTOs;
using Microsoft.AspNetCore.Identity;

namespace ControlInventariosAPI.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<IdentityUser, UsuarioDTO>();
        }
    }
}
