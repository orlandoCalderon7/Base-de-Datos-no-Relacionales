namespace AuthService.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Rol { get; set; } = "Adoptante"; // Admin, Albergue, Adoptante
    public int? AlbergueId { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}
