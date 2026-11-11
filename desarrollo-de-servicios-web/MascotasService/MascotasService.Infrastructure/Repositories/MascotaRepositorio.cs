namespace MascotasService.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using MascotasService.Application.Interfaces;
using MascotasService.Domain.Entities;
using MascotasService.Infrastructure.Data;

public class MascotaRepositorio(AdoptaMasDbContext context) : IMascotaRepositorio
{
    public async Task<List<Mascota>> ObtenerTodosAsync(string? especie = null, string? estado = null, int? albergueId = null)
    {
        var query = context.Mascotas
            .Include(m => m.Albergue)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(especie))
            query = query.Where(m => m.Especie.ToLower() == especie.ToLower());

        if (!string.IsNullOrWhiteSpace(estado))
            query = query.Where(m => m.Estado.ToLower() == estado.ToLower());

        if (albergueId.HasValue && albergueId.Value > 0)
            query = query.Where(m => m.AlbergueId == albergueId.Value);

        return await query.OrderByDescending(m => m.FechaIngreso).ToListAsync();
    }

    public async Task<Mascota?> ObtenerPorIdAsync(int id)
    {
        return await context.Mascotas
            .Include(m => m.Albergue)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Mascota> AgregarAsync(Mascota mascota)
    {
        context.Mascotas.Add(mascota);
        await context.SaveChangesAsync();
        return mascota;
    }

    public async Task<bool> ActualizarAsync(Mascota mascota)
    {
        context.Entry(mascota).State = EntityState.Modified;
        return (await context.SaveChangesAsync()) > 0;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var mascota = await context.Mascotas.FindAsync(id);
        if (mascota is null) return false;

        context.Mascotas.Remove(mascota);
        return (await context.SaveChangesAsync()) > 0;
    }
}
