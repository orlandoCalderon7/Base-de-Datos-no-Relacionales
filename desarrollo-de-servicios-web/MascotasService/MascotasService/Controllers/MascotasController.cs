namespace MascotasService.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MascotasService.Application.DTOs;
using MascotasService.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class MascotasController(IMascotaServicio servicio) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MascotaRespuestaDto>))]
    public async Task<IActionResult> ObtenerTodas(
        [FromQuery] string? especie,
        [FromQuery] string? estado,
        [FromQuery] int? albergueId)
    {
        var resultados = await servicio.ListarMascotasAsync(especie, estado, albergueId);
        return Ok(resultados);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MascotaRespuestaDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var mascota = await servicio.BuscarPorIdAsync(id);
        return mascota is null 
            ? NotFound(new { mensaje = $"Mascota con ID {id} no encontrada." }) 
            : Ok(mascota);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Albergue")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MascotaRespuestaDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Crear([FromBody] CrearMascotaDto dto)
    {
        var nueva = await servicio.RegistrarAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = nueva.Id }, nueva);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Albergue")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarMascotaDto dto)
    {
        var actualizado = await servicio.ActualizarAsync(id, dto);
        if (!actualizado) 
            return NotFound(new { mensaje = $"Mascota con ID {id} no existe." });

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await servicio.EliminarAsync(id);
        if (!eliminado) 
            return NotFound(new { mensaje = $"Mascota con ID {id} no existe." });

        return NoContent();
    }
}
