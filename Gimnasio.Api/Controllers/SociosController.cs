using Gimnasio.Entidades.Dtos;
using Gimnasio.Negocio.Excepciones;
using Gimnasio.Negocio.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Gimnasio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SociosController(ISocioServicio socioServicio, IMembresiaServicio membresiaServicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SocioDto>>> GetSocios(
        [FromQuery] string? nombre,
        [FromQuery] string? dni)
    {
        return Ok(await socioServicio.ObtenerTodosAsync(nombre, dni));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SocioDto>> GetSocio(int id)
    {
        var socio = await socioServicio.ObtenerPorIdAsync(id);

        if (socio is null)
            return NotFound();

        return Ok(socio);
    }

    [HttpGet("{id:int}/membresias")]
    public async Task<ActionResult<IEnumerable<MembresiaDto>>> GetMembresiasDeSocio(int id)
    {
        var membresias = await membresiaServicio.ObtenerPorSocioAsync(id);

        if (membresias is null)
            return NotFound();

        return Ok(membresias);
    }

    [HttpPost]
    public async Task<ActionResult<SocioDto>> PostSocio(SocioCrearDto dto)
    {
        try
        {
            var socio = await socioServicio.CrearAsync(dto);

            return CreatedAtAction(nameof(GetSocio), new { id = socio.Id }, socio);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutSocio(int id, SocioActualizarDto dto)
    {
        try
        {
            if (!await socioServicio.ActualizarAsync(id, dto))
                return NotFound();

            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSocio(int id)
    {
        try
        {
            if (!await socioServicio.EliminarAsync(id))
                return NotFound();

            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }
}