using System.ComponentModel.DataAnnotations;

namespace Gimnasio.Entidades.Dtos;

public class SocioActualizarDto
{
    [Required(ErrorMessage = "El DNI es obligatorio")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener 8 dígitos")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI solo admite números")]
    public string Dni { get; set; } = null!;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    public string Nombre { get; set; } = null!;

    [StringLength(15, ErrorMessage = "El teléfono admite hasta 15 caracteres")]
    public string? Telefono { get; set; }

    [StringLength(100, ErrorMessage = "El email admite hasta 100 caracteres")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string? Email { get; set; }
}