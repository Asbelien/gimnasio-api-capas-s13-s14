using Gimnasio.Entidades.Dtos;

namespace Gimnasio.Negocio.Servicios;

public interface IMembresiaServicio
{
    Task<List<MembresiaDto>> ObtenerTodasAsync(string? tipoPlan, bool? vigente);

    Task<List<MembresiaDto>?> ObtenerPorSocioAsync(int socioId);

    Task<MembresiaDto?> ObtenerPorIdAsync(int id);

    Task<MembresiaDto> CrearAsync(MembresiaCrearDto dto);

    Task<bool> ActualizarAsync(int id, MembresiaActualizarDto dto);

    Task<bool> EliminarAsync(int id);
}