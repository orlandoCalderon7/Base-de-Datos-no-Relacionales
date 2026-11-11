namespace AuthService.Application.DTOs;

using System.ComponentModel.DataAnnotations;

public class RegistroDto
{
    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [StringLength(80, MinimumLength = 3)]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    public string Password { get; set; } = string.Empty;

    [Phone]
    public string Telefono { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^(Admin|Albergue|Adoptante)$", ErrorMessage = "El rol debe ser 'Admin', 'Albergue' o 'Adoptante'.")]
    public string Rol { get; set; } = "Adoptante";

    public int? AlbergueId { get; set; }
}

public class LoginDto
{
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string Password { get; set; } = string.Empty;
}

public record AuthRespuestaDto(
    int UsuarioId,
    string NombreCompleto,
    string Email,
    string Rol,
    int? AlbergueId,
    string Token,
    DateTime ExpiraEnUtc
);
