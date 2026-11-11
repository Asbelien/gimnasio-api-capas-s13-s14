using AutoMapper;
using Gimnasio.Entidades;
using Gimnasio.Entidades.Dtos;

namespace Gimnasio.Negocio.Mapeos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Socio, SocioDto>();
        CreateMap<SocioCrearDto, Socio>();
        CreateMap<SocioActualizarDto, Socio>();

        CreateMap<Membresia, MembresiaDto>()
            .ForMember(destino => destino.SocioNombre, opciones => opciones.MapFrom(origen => origen.Socio.Nombre));
        CreateMap<MembresiaCrearDto, Membresia>();
        CreateMap<MembresiaActualizarDto, Membresia>();
    }
}