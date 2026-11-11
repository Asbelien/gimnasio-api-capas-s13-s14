using System.ComponentModel.DataAnnotations;

namespace Gimnasio.Entidades.Dtos;

public class MembresiaCrearDto
{
    [Required(ErrorMessage = "El tipo de plan es obligatorio")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "El tipo de plan debe tener entre 3 y 30 caracteres")]
    public string TipoPlan { get; set; } = null!;

    [Range(0.01, 99999.99, ErrorMessage = "El precio debe estar entre 0.01 y 99999.99")]
    public decimal Precio { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El socio es obligatorio")]
    public int SocioId { get; set; }
}