namespace MascotasService.Application.Interfaces;

using MascotasService.Domain.Entities;

public interface IMascotaRepositorio
{
    Task<List<Mascota>> ObtenerTodosAsync(string? especie = null, string? estado = null, int? albergueId = null);
    Task<Mascota?> ObtenerPorIdAsync(int id);
    Task<Mascota> AgregarAsync(Mascota mascota);
    Task<bool> ActualizarAsync(Mascota mascota);
    Task<bool> EliminarAsync(int id);
}
