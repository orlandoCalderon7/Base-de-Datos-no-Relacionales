namespace MascotasService.Application.DTOs;

using System.ComponentModel.DataAnnotations;

public class CrearSolicitudDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "ID de mascota inválido.")]
    public int MascotaId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "ID de usuario inválido.")]
    public int UsuarioId { get; set; }

    public bool TienePatio { get; set; }
    public bool TieneOtrasMascotas { get; set; }

    [Required(ErrorMessage = "Debe especificar el motivo de adopción.")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "El motivo debe tener entre 10 y 500 caracteres.")]
    public string MotivoAdopcion { get; set; } = string.Empty;
}

public record SolicitudRespuestaDto(
    int Id,
    int MascotaId,
    string? MascotaNombre,
    int UsuarioId,
    DateTime FechaSolicitud,
    string Estado,
    bool TienePatio,
    bool TieneOtrasMascotas,
    string MotivoAdopcion,
    string ComentariosAlbergue
);
