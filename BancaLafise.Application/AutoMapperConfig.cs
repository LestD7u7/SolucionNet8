using AutoMapper;
using BancaLafise.Application.Common.Dtos;
using BancaLafise.Domain.Entities;

namespace BancaLafise.Application
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() 
        {
            CreateMap<ClienteDto, Cliente>();
            CreateMap<UsuarioDto, Usuario>();

        }
    }
}
