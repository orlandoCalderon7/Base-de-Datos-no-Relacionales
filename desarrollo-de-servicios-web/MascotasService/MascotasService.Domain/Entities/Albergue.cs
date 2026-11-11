namespace MascotasService.Domain.Entities;

public class Albergue
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Ciudad { get; set; } = "Lima";
    public string Telefono { get; set; } = string.Empty;
    public string EmailContacto { get; set; } = string.Empty;
    public int CapacidadMaxima { get; set; } = 50;
    public bool EsActivo { get; set; } = true;

    // Relación 1 a N
    public List<Mascota> Mascotas { get; set; } = [];
}
