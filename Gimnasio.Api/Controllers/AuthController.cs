using Gimnasio.Entidades.Dtos;
using Gimnasio.Negocio.Excepciones;
using Gimnasio.Negocio.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gimnasio.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthServicio servicio) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("registro")]
    public async Task<IActionResult> Registro(RegistroDto dto)
    {
        try
        {
            var perfil = await servicio.RegistrarAsync(dto);

            return StatusCode(StatusCodes.Status201Created, perfil);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await servicio.LoginAsync(dto);

        if (token is null)
            return Unauthorized(new { mensaje = "Credenciales inválidas" });

        return Ok(token);
    }

    [Authorize]
    [HttpGet("perfil")]
    public IActionResult Perfil()
    {
        var perfil = new PerfilDto
        {
            Correo = User.FindFirst("email")?.Value ?? string.Empty,
            Rol = User.FindFirst("role")?.Value ?? string.Empty
        };

        return Ok(perfil);
    }
}