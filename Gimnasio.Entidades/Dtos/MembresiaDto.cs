namespace Gimnasio.Entidades.Dtos;

public class MembresiaDto
{
    public int Id { get; set; }

    public string TipoPlan { get; set; } = null!;

    public decimal Precio { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public int SocioId { get; set; }

    public string SocioNombre { get; set; } = null!;
}