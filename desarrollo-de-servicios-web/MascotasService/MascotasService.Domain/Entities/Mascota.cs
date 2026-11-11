namespace MascotasService.Domain.Entities;

public class Mascota
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Especie { get; set; } = "Perro"; // Perro, Gato, Conejo, Ave, etc.
    public string Raza { get; set; } = "Mestizo";
    public int EdadMeses { get; set; }
    public string Sexo { get; set; } = "Macho"; // Macho, Hembra
    public string Tamano { get; set; } = "Mediano"; // Pequeno, Mediano, Grande
    public string Estado { get; set; } = "Disponible"; // Disponible, EnProceso, Adoptado
    public string Descripcion { get; set; } = string.Empty;
    public string FotoUrl { get; set; } = string.Empty;
    public int AlbergueId { get; set; }
    public DateTime FechaIngreso { get; set; } = DateTime.UtcNow;

    // Relaciones
    public Albergue? Albergue { get; set; }
    public List<SolicitudAdopcion> Solicitudes { get; set; } = [];
}
