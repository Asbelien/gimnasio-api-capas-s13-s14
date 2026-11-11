using AutoMapper;
using Gimnasio.Datos.Repositorios;
using Gimnasio.Entidades;
using Gimnasio.Entidades.Dtos;
using Gimnasio.Negocio.Excepciones;

namespace Gimnasio.Negocio.Servicios;

public class SocioServicio(ISocioRepositorio repositorio, IMapper mapper) : ISocioServicio
{
    public async Task<List<SocioDto>> ObtenerTodosAsync(string? nombre, string? dni)
    {
        var socios = await repositorio.ObtenerTodosAsync(nombre, dni);

        return mapper.Map<List<SocioDto>>(socios);
    }

    public async Task<SocioDto?> ObtenerPorIdAsync(int id)
    {
        var socio = await repositorio.ObtenerPorIdAsync(id);

        return socio is null ? null : mapper.Map<SocioDto>(socio);
    }

    public async Task<SocioDto> CrearAsync(SocioCrearDto dto)
    {
        if (await repositorio.ExisteDniAsync(dto.Dni))
            throw new ReglaNegocioException($"Ya existe un socio con el DNI {dto.Dni}");

        var socio = mapper.Map<Socio>(dto);

        await repositorio.AgregarAsync(socio);
        await repositorio.GuardarCambiosAsync();

        return mapper.Map<SocioDto>(socio);
    }

    public async Task<bool> ActualizarAsync(int id, SocioActualizarDto dto)
    {
        var socio = await repositorio.ObtenerPorIdAsync(id);

        if (socio is null)
            return false;

        if (await repositorio.ExisteDniAsync(dto.Dni, id))
            throw new ReglaNegocioException($"Ya existe otro socio con el DNI {dto.Dni}");

        mapper.Map(dto, socio);

        await repositorio.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var socio = await repositorio.ObtenerPorIdAsync(id);

        if (socio is null)
            return false;

        if (await repositorio.TieneMembresiasAsync(id))
            throw new ReglaNegocioException("No se puede eliminar un socio que tiene membresías registradas");

        repositorio.Eliminar(socio);
        await repositorio.GuardarCambiosAsync();

        return true;
    }
}