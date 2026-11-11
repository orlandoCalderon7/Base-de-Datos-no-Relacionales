namespace MascotasService.Application.DTOs;

using System.ComponentModel.DataAnnotations;

public class CrearAlbergueDto
{
    [Required(ErrorMessage = "El nombre del albergue es obligatorio.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Nombre entre 3 y 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La dirección es obligatoria.")]
    public string Direccion { get; set; } = string.Empty;

    [StringLength(50)]
    public string Ciudad { get; set; } = "Lima";

    [Phone(ErrorMessage = "Teléfono no válido.")]
    public string Telefono { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Email de contacto no válido.")]
    public string EmailContacto { get; set; } = string.Empty;

    [Range(1, 500, ErrorMessage = "Capacidad entre 1 y 500 mascotas.")]
    public int CapacidadMaxima { get; set; } = 50;
}

public record AlbergueRespuestaDto(
    int Id,
    string Nombre,
    string Direccion,
    string Ciudad,
    string Telefono,
    string EmailContacto,
    int CapacidadMaxima,
    bool EsActivo,
    int TotalMascotas
);
