using AutoMapper;
using Gimnasio.Datos.Repositorios;
using Gimnasio.Entidades;
using Gimnasio.Entidades.Dtos;
using Gimnasio.Negocio.Excepciones;

namespace Gimnasio.Negocio.Servicios;

public class MembresiaServicio(
    IMembresiaRepositorio repositorio,
    ISocioRepositorio socioRepositorio,
    IMapper mapper) : IMembresiaServicio
{
    public async Task<List<MembresiaDto>> ObtenerTodasAsync(string? tipoPlan, bool? vigente)
    {
        var membresias = await repositorio.ObtenerTodasAsync(tipoPlan, vigente);

        return mapper.Map<List<MembresiaDto>>(membresias);
    }

    public async Task<List<MembresiaDto>?> ObtenerPorSocioAsync(int socioId)
    {
        if (!await socioRepositorio.ExisteAsync(socioId))
            return null;

        var membresias = await repositorio.ObtenerPorSocioAsync(socioId);

        return mapper.Map<List<MembresiaDto>>(membresias);
    }

    public async Task<MembresiaDto?> ObtenerPorIdAsync(int id)
    {
        var membresia = await repositorio.ObtenerPorIdAsync(id);

        return membresia is null ? null : mapper.Map<MembresiaDto>(membresia);
    }

    public async Task<MembresiaDto> CrearAsync(MembresiaCrearDto dto)
    {
        var socio = await socioRepositorio.ObtenerPorIdAsync(dto.SocioId)
            ?? throw new ReglaNegocioException($"No existe un socio con id {dto.SocioId}");

        if (dto.FechaFin < dto.FechaInicio)
            throw new ReglaNegocioException("La fecha de fin no puede ser anterior a la fecha de inicio");

        if (await repositorio.ExisteSolapamientoAsync(dto.SocioId, dto.FechaInicio, dto.FechaFin))
            throw new ReglaNegocioException("El socio ya tiene una membresía que se cruza con esas fechas");

        var membresia = mapper.Map<Membresia>(dto);
        membresia.Socio = socio;

        await repositorio.AgregarAsync(membresia);
        await repositorio.GuardarCambiosAsync();

        return mapper.Map<MembresiaDto>(membresia);
    }

    public async Task<bool> ActualizarAsync(int id, MembresiaActualizarDto dto)
    {
        var membresia = await repositorio.ObtenerPorIdAsync(id);

        if (membresia is null)
            return false;

        if (!await socioRepositorio.ExisteAsync(dto.SocioId))
            throw new ReglaNegocioException($"No existe un socio con id {dto.SocioId}");

        if (dto.FechaFin < dto.FechaInicio)
            throw new ReglaNegocioException("La fecha de fin no puede ser anterior a la fecha de inicio");

        if (await repositorio.ExisteSolapamientoAsync(dto.SocioId, dto.FechaInicio, dto.FechaFin, id))
            throw new ReglaNegocioException("El socio ya tiene una membresía que se cruza con esas fechas");

        mapper.Map(dto, membresia);

        await repositorio.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var membresia = await repositorio.ObtenerPorIdAsync(id);

        if (membresia is null)
            return false;

        repositorio.Eliminar(membresia);
        await repositorio.GuardarCambiosAsync();

        return true;
    }
}