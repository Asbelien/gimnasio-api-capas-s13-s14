namespace Gimnasio.Entidades;

public partial class Socio
{
    public int Id { get; set; }

    public string Dni { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Membresia> Membresia { get; set; } = new List<Membresia>();
}