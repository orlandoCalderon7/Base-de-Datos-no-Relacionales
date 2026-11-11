namespace MascotasService.Domain.Entities;

public class SolicitudAdopcion
{
    public int Id { get; set; }
    public int MascotaId { get; set; }
    public int UsuarioId { get; set; }
    public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;
    public string Estado { get; set; } = "Pendiente"; // Pendiente, Aprobada, Rechazada
    public bool TienePatio { get; set; }
    public bool TieneOtrasMascotas { get; set; }
    public string MotivoAdopcion { get; set; } = string.Empty;
    public string ComentariosAlbergue { get; set; } = string.Empty;

    // Navegación
    public Mascota? Mascota { get; set; }
}
