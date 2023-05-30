using AutoMapper;
using ControlInventariosAPI.DTOs;
using ControlInventariosAPI.Entidades;
using Microsoft.AspNetCore.Identity;

namespace ControlInventariosAPI.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<IdentityUser, UsuarioDTO>();
            CreateMap<Articulo, ArticuloDTO>().ReverseMap();
            CreateMap<Pedido, PedidoDTO>().ReverseMap();
        }
    }
}
