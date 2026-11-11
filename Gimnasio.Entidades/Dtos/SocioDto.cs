namespace Gimnasio.Entidades.Dtos;

public class SocioDto
{
    public int Id { get; set; }

    public string Dni { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Email { get; set; }
}