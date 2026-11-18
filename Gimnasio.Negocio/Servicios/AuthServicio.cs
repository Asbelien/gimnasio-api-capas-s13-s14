using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gimnasio.Datos.Repositorios;
using Gimnasio.Entidades;
using Gimnasio.Entidades.Dtos;
using Gimnasio.Negocio.Excepciones;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Gimnasio.Negocio.Servicios;

public class AuthServicio(IUsuarioRepositorio repositorio, IConfiguration configuracion) : IAuthServicio
{
    private const string RolPorDefecto = "Entrenador";

    public async Task<PerfilDto> RegistrarAsync(RegistroDto dto)
    {
        var correo = dto.Correo.Trim().ToLowerInvariant();

        if (await repositorio.ObtenerPorCorreoAsync(correo) is not null)
            throw new ReglaNegocioException($"Ya existe un usuario con el correo {correo}");

        var usuario = new Usuario
        {
            Correo = correo,
            ClaveHash = BCrypt.Net.BCrypt.HashPassword(dto.Clave),
            Rol = RolPorDefecto
        };

        await repositorio.AgregarAsync(usuario);
        await repositorio.GuardarCambiosAsync();

        return new PerfilDto { Correo = usuario.Correo, Rol = usuario.Rol };
    }

    public async Task<TokenDto?> LoginAsync(LoginDto dto)
    {
        var correo = dto.Correo.Trim().ToLowerInvariant();

        var usuario = await repositorio.ObtenerPorCorreoAsync(correo);

        // Misma respuesta (null) si el correo no existe o la clave es incorrecta
        if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Clave, usuario.ClaveHash))
            return null;

        return GenerarToken(usuario);
    }

    private TokenDto GenerarToken(Usuario usuario)
    {
        var jwt = configuracion.GetSection("Jwt");

        var expiracion = DateTime.UtcNow.AddMinutes(int.Parse(jwt["Minutos"]!));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
            new Claim("role", usuario.Rol)
        };

        var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Clave"]!));

        var token = new JwtSecurityToken(
            issuer: jwt["Emisor"],
            audience: jwt["Audiencia"],
            claims: claims,
            expires: expiracion,
            signingCredentials: new SigningCredentials(clave, SecurityAlgorithms.HmacSha256));

        return new TokenDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiracion = expiracion
        };
    }
}