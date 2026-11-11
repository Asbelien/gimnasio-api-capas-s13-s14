using Gimnasio.Entidades.Dtos;

namespace Gimnasio.Negocio.Servicios;

public interface ISocioServicio
{
    Task<List<SocioDto>> ObtenerTodosAsync(string? nombre, string? dni);

    Task<SocioDto?> ObtenerPorIdAsync(int id);

    Task<SocioDto> CrearAsync(SocioCrearDto dto);

    Task<bool> ActualizarAsync(int id, SocioActualizarDto dto);

    Task<bool> EliminarAsync(int id);
}