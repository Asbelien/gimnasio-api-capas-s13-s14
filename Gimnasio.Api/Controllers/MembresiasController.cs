using Gimnasio.Entidades.Dtos;
using Gimnasio.Negocio.Excepciones;
using Gimnasio.Negocio.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gimnasio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MembresiasController(IMembresiaServicio membresiaServicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MembresiaDto>>> GetMembresias(
        [FromQuery] string? tipoPlan,
        [FromQuery] bool? vigente)
    {
        return Ok(await membresiaServicio.ObtenerTodasAsync(tipoPlan, vigente));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MembresiaDto>> GetMembresia(int id)
    {
        var membresia = await membresiaServicio.ObtenerPorIdAsync(id);

        if (membresia is null)
            return NotFound();

        return Ok(membresia);
    }

    [HttpPost]
    [Authorize(Roles = "Recepcionista")]
    public async Task<ActionResult<MembresiaDto>> PostMembresia(MembresiaCrearDto dto)
    {
        try
        {
            var membresia = await membresiaServicio.CrearAsync(dto);

            return CreatedAtAction(nameof(GetMembresia), new { id = membresia.Id }, membresia);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Recepcionista")]
    public async Task<IActionResult> PutMembresia(int id, MembresiaActualizarDto dto)
    {
        try
        {
            if (!await membresiaServicio.ActualizarAsync(id, dto))
                return NotFound();

            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Recepcionista")]
    public async Task<IActionResult> DeleteMembresia(int id)
    {
        if (!await membresiaServicio.EliminarAsync(id))
            return NotFound();

        return NoContent();
    }
}