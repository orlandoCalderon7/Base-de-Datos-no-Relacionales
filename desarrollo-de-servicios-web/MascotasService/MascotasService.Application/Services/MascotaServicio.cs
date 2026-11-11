namespace MascotasService.Application.Services;

using MascotasService.Application.DTOs;
using MascotasService.Application.Interfaces;
using MascotasService.Domain.Entities;

public class MascotaServicio(IMascotaRepositorio repositorio) : IMascotaServicio
{
    public async Task<List<MascotaRespuestaDto>> ListarMascotasAsync(string? especie, string? estado, int? albergueId)
    {
        var mascotas = await repositorio.ObtenerTodosAsync(especie, estado, albergueId);
        return mascotas.Select(MapearADto).ToList();
    }

    public async Task<MascotaRespuestaDto?> BuscarPorIdAsync(int id)
    {
        var m = await repositorio.ObtenerPorIdAsync(id);
        return m is null ? null : MapearADto(m);
    }

    public async Task<MascotaRespuestaDto> RegistrarAsync(CrearMascotaDto dto)
    {
        var mascota = new Mascota
        {
            Nombre = dto.Nombre,
            Especie = dto.Especie,
            Raza = dto.Raza,
            EdadMeses = dto.EdadMeses,
            Sexo = dto.Sexo,
            Tamano = dto.Tamano,
            Estado = "Disponible",
            Descripcion = dto.Descripcion,
            FotoUrl = dto.FotoUrl,
            AlbergueId = dto.AlbergueId,
            FechaIngreso = DateTime.UtcNow
        };

        var creada = await repositorio.AgregarAsync(mascota);
        return MapearADto(creada);
    }

    public async Task<bool> ActualizarAsync(int id, ActualizarMascotaDto dto)
    {
        var existente = await repositorio.ObtenerPorIdAsync(id);
        if (existente is null) return false;

        existente.Nombre = dto.Nombre;
        existente.Especie = dto.Especie;
        existente.Raza = dto.Raza;
        existente.EdadMeses = dto.EdadMeses;
        existente.Sexo = dto.Sexo;
        existente.Tamano = dto.Tamano;
        existente.Estado = dto.Estado;
        existente.Descripcion = dto.Descripcion;
        existente.FotoUrl = dto.FotoUrl;
        existente.AlbergueId = dto.AlbergueId;

        return await repositorio.ActualizarAsync(existente);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        return await repositorio.EliminarAsync(id);
    }

    private static MascotaRespuestaDto MapearADto(Mascota m) =>
        new(
            m.Id,
            m.Nombre,
            m.Especie,
            m.Raza,
            m.EdadMeses,
            m.Sexo,
            m.Tamano,
            m.Estado,
            m.Descripcion,
            m.FotoUrl,
            m.AlbergueId,
            m.Albergue?.Nombre,
            m.FechaIngreso
        );
}
