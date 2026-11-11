namespace MascotasService.Application.Interfaces;

using MascotasService.Application.DTOs;

public interface IMascotaServicio
{
    Task<List<MascotaRespuestaDto>> ListarMascotasAsync(string? especie, string? estado, int? albergueId);
    Task<MascotaRespuestaDto?> BuscarPorIdAsync(int id);
    Task<MascotaRespuestaDto> RegistrarAsync(CrearMascotaDto dto);
    Task<bool> ActualizarAsync(int id, ActualizarMascotaDto dto);
    Task<bool> EliminarAsync(int id);
}
