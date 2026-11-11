namespace MascotasService.Application.DTOs;

using System.ComponentModel.DataAnnotations;

public class CrearMascotaDto
{
    [Required(ErrorMessage = "El nombre de la mascota es obligatorio.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La especie es obligatoria (Perro, Gato, etc.).")]
    public string Especie { get; set; } = string.Empty;

    [StringLength(50)]
    public string Raza { get; set; } = "Mestizo";

    [Range(1, 240, ErrorMessage = "La edad debe estar comprendida entre 1 y 240 meses (20 años).")]
    public int EdadMeses { get; set; }

    [Required]
    [RegularExpression("^(Macho|Hembra)$", ErrorMessage = "El sexo debe ser 'Macho' o 'Hembra'.")]
    public string Sexo { get; set; } = "Macho";

    [Required]
    [RegularExpression("^(Pequeno|Mediano|Grande)$", ErrorMessage = "El tamaño debe ser 'Pequeno', 'Mediano' o 'Grande'.")]
    public string Tamano { get; set; } = "Mediano";

    [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
    public string Descripcion { get; set; } = string.Empty;

    [Url(ErrorMessage = "La foto debe ser una URL válida.")]
    public string FotoUrl { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Debe asignar un ID de albergue válido.")]
    public int AlbergueId { get; set; }
}

public class ActualizarMascotaDto : CrearMascotaDto
{
    [Required]
    [RegularExpression("^(Disponible|EnProceso|Adoptado)$", ErrorMessage = "Estado no válido.")]
    public string Estado { get; set; } = "Disponible";
}

public record MascotaRespuestaDto(
    int Id,
    string Nombre,
    string Especie,
    string Raza,
    int EdadMeses,
    string Sexo,
    string Tamano,
    string Estado,
    string Descripcion,
    string FotoUrl,
    int AlbergueId,
    string? AlbergueNombre,
    DateTime FechaIngreso
);
