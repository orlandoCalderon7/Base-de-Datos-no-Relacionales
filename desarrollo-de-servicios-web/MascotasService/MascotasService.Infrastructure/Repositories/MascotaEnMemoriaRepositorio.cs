namespace MascotasService.Infrastructure.Repositories;

using MascotasService.Application.Interfaces;
using MascotasService.Domain.Entities;

public class MascotaEnMemoriaRepositorio : IMascotaRepositorio
{
    private readonly List<Mascota> _mascotas =
    [
        new Mascota { Id = 1, Nombre = "Bobby", Especie = "Perro", Raza = "Mestizo", EdadMeses = 18, Sexo = "Macho", Tamano = "Mediano", Estado = "Disponible", AlbergueId = 1 },
        new Mascota { Id = 2, Nombre = "Michi", Especie = "Gato", Raza = "Común", EdadMeses = 8, Sexo = "Hembra", Tamano = "Pequeno", Estado = "Disponible", AlbergueId = 1 },
        new Mascota { Id = 3, Nombre = "Luna", Especie = "Perro", Raza = "Poodle", EdadMeses = 36, Sexo = "Hembra", Tamano = "Pequeno", Estado = "Adoptado", AlbergueId = 2 }
    ];
    private int _siguienteId = 4;

    public Task<List<Mascota>> ObtenerTodosAsync(string? especie = null, string? estado = null, int? albergueId = null)
    {
        var query = _mascotas.AsQueryable();

        if (!string.IsNullOrWhiteSpace(especie))
            query = query.Where(m => m.Especie.Equals(especie, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(estado))
            query = query.Where(m => m.Estado.Equals(estado, StringComparison.OrdinalIgnoreCase));

        if (albergueId.HasValue && albergueId.Value > 0)
            query = query.Where(m => m.AlbergueId == albergueId.Value);

        return Task.FromResult(query.OrderByDescending(m => m.FechaIngreso).ToList());
    }

    public Task<Mascota?> ObtenerPorIdAsync(int id)
    {
        var item = _mascotas.FirstOrDefault(m => m.Id == id);
        return Task.FromResult(item);
    }

    public Task<Mascota> AgregarAsync(Mascota mascota)
    {
        mascota.Id = _siguienteId++;
        mascota.FechaIngreso = DateTime.UtcNow;
        _mascotas.Add(mascota);
        return Task.FromResult(mascota);
    }

    public Task<bool> ActualizarAsync(Mascota mascota)
    {
        var indice = _mascotas.FindIndex(m => m.Id == mascota.Id);
        if (indice == -1) return Task.FromResult(false);

        _mascotas[indice] = mascota;
        return Task.FromResult(true);
    }

    public Task<bool> EliminarAsync(int id)
    {
        var item = _mascotas.FirstOrDefault(m => m.Id == id);
        if (item is null) return Task.FromResult(false);

        _mascotas.Remove(item);
        return Task.FromResult(true);
    }
}
