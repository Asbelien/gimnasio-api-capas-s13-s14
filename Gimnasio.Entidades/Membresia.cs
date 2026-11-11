namespace Gimnasio.Entidades;

public partial class Membresia
{
    public int Id { get; set; }

    public string TipoPlan { get; set; } = null!;

    public decimal Precio { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public int SocioId { get; set; }

    public virtual Socio Socio { get; set; } = null!;
}