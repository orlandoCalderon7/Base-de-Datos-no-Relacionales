namespace MascotasService.Application.Interfaces;

using MascotasService.Application.DTOs;

public interface ISolicitudServicio
{
    Task<List<SolicitudRespuestaDto>> ListarSolicitudesAsync(int? mascotaId, int? usuarioId, string? estado);
    Task<SolicitudRespuestaDto?> BuscarPorIdAsync(int id);
    Task<SolicitudRespuestaDto> CrearSolicitudAsync(CrearSolicitudDto dto);
    Task<bool> AprobarSolicitudAdopcionAsync(int solicitudId);
    Task<bool> RechazarSolicitudAdopcionAsync(int solicitudId, string motivo);
}
