using System.ComponentModel.DataAnnotations;

namespace Gimnasio.Entidades.Dtos;

public class RegistroDto
{
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [StringLength(150)]
    public string Correo { get; set; } = null!;

    [Required(ErrorMessage = "La clave es obligatoria.")]
    [MinLength(6, ErrorMessage = "La clave debe tener al menos 6 caracteres.")]
    public string Clave { get; set; } = null!;
}