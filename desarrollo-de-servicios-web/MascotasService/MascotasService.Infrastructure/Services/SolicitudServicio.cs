namespace MascotasService.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MascotasService.Application.DTOs;
using MascotasService.Application.Interfaces;
using MascotasService.Domain.Entities;
using MascotasService.Infrastructure.Data;

public class SolicitudServicio(AdoptaMasDbContext context, ILogger<SolicitudServicio> logger) : ISolicitudServicio
{
    public async Task<List<SolicitudRespuestaDto>> ListarSolicitudesAsync(int? mascotaId, int? usuarioId, string? estado)
    {
        var query = context.Solicitudes
            .Include(s => s.Mascota)
            .AsNoTracking()
            .AsQueryable();

        if (mascotaId.HasValue)
            query = query.Where(s => s.MascotaId == mascotaId.Value);

        if (usuarioId.HasValue)
            query = query.Where(s => s.UsuarioId == usuarioId.Value);

        if (!string.IsNullOrWhiteSpace(estado))
            query = query.Where(s => s.Estado.ToLower() == estado.ToLower());

        var list = await query.OrderByDescending(s => s.FechaSolicitud).ToListAsync();
        return list.Select(MapearADto).ToList();
    }

    public async Task<SolicitudRespuestaDto?> BuscarPorIdAsync(int id)
    {
        var s = await context.Solicitudes
            .Include(s => s.Mascota)
            .FirstOrDefaultAsync(s => s.Id == id);

        return s is null ? null : MapearADto(s);
    }

    public async Task<SolicitudRespuestaDto> CrearSolicitudAsync(CrearSolicitudDto dto)
    {
        var mascota = await context.Mascotas.FindAsync(dto.MascotaId)
            ?? throw new InvalidOperationException($"La mascota con ID {dto.MascotaId} no existe.");

        if (mascota.Estado != "Disponible")
            throw new InvalidOperationException($"La mascota '{mascota.Nombre}' no está disponible para adopción.");

        var solicitud = new SolicitudAdopcion
        {
            MascotaId = dto.MascotaId,
            UsuarioId = dto.UsuarioId,
            FechaSolicitud = DateTime.UtcNow,
            Estado = "Pendiente",
            TienePatio = dto.TienePatio,
            TieneOtrasMascotas = dto.TieneOtrasMascotas,
            MotivoAdopcion = dto.MotivoAdopcion
        };

        context.Solicitudes.Add(solicitud);
        await context.SaveChangesAsync();

        return MapearADto(solicitud);
    }

    /// <summary>
    /// Transacción ACID de Negocio:
    /// 1. Aprueba la solicitud seleccionada.
    /// 2. Actualiza el estado de la Mascota a 'Adoptado'.
    /// 3. Rechaza automáticamente todas las demás solicitudes pendientes para esa mascota.
    /// </summary>
    public async Task<bool> AprobarSolicitudAdopcionAsync(int solicitudId)
    {
        using var transaccion = await context.Database.BeginTransactionAsync();
        try
        {
            var solicitud = await context.Solicitudes
                .Include(s => s.Mascota)
                .FirstOrDefaultAsync(s => s.Id == solicitudId);

            if (solicitud is null || solicitud.Estado != "Pendiente")
            {
                logger.LogWarning("Solicitud {Id} no válida o no está pendiente.", solicitudId);
                return false;
            }

            var mascota = solicitud.Mascota;
            if (mascota is null || mascota.Estado != "Disponible")
            {
                logger.LogWarning("Mascota asociada {MascotaId} ya no está disponible.", solicitud.MascotaId);
                return false;
            }

            // Paso A: Aprobar solicitud
            solicitud.Estado = "Aprobada";
            solicitud.ComentariosAlbergue = "Adopción aprobada formalmente por el albergue.";

            // Paso B: Marcar mascota como adoptada
            mascota.Estado = "Adoptado";

            // Paso C: Rechazar concurrentemente las demás solicitudes pendientes
            var otrasPendientes = await context.Solicitudes
                .Where(s => s.MascotaId == mascota.Id && s.Id != solicitudId && s.Estado == "Pendiente")
                .ToListAsync();

            foreach (var otra in otrasPendientes)
            {
                otra.Estado = "Rechazada";
                otra.ComentariosAlbergue = "Mascota entregada a otro adoptante con prioridad.";
            }

            await context.SaveChangesAsync();
            await transaccion.CommitAsync();

            logger.LogInformation("Transacción ACID completada: Solicitud {Id} aprobada y Mascota {MascotaId} adoptada.", solicitudId, mascota.Id);
            return true;
        }
        catch (Exception ex)
        {
            await transaccion.RollbackAsync();
            logger.LogError(ex, "Error en la transacción ACID de adopción para la solicitud {Id}. Rollback ejecutado.", solicitudId);
            throw;
        }
    }

    public async Task<bool> RechazarSolicitudAdopcionAsync(int solicitudId, string motivo)
    {
        var solicitud = await context.Solicitudes.FindAsync(solicitudId);
        if (solicitud is null || solicitud.Estado != "Pendiente") return false;

        solicitud.Estado = "Rechazada";
        solicitud.ComentariosAlbergue = string.IsNullOrWhiteSpace(motivo) ? "Solicitud no cumple con los requisitos del albergue." : motivo;

        await context.SaveChangesAsync();
        return true;
    }

    private static SolicitudRespuestaDto MapearADto(SolicitudAdopcion s) =>
        new(
            s.Id,
            s.MascotaId,
            s.Mascota?.Nombre,
            s.UsuarioId,
            s.FechaSolicitud,
            s.Estado,
            s.TienePatio,
            s.TieneOtrasMascotas,
            s.MotivoAdopcion,
            s.ComentariosAlbergue
        );
}
