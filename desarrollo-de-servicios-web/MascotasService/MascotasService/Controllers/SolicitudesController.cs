namespace MascotasService.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MascotasService.Application.DTOs;
using MascotasService.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class SolicitudesController(ISolicitudServicio servicio) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin,Albergue")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SolicitudRespuestaDto>))]
    public async Task<IActionResult> ObtenerTodas(
        [FromQuery] int? mascotaId,
        [FromQuery] int? usuarioId,
        [FromQuery] string? estado)
    {
        var resultados = await servicio.ListarSolicitudesAsync(mascotaId, usuarioId, estado);
        return Ok(resultados);
    }

    [HttpGet("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SolicitudRespuestaDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var s = await servicio.BuscarPorIdAsync(id);
        return s is null ? NotFound(new { mensaje = $"Solicitud {id} no encontrada." }) : Ok(s);
    }

    [HttpPost]
    [Authorize(Roles = "Adoptante,Admin")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SolicitudRespuestaDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear([FromBody] CrearSolicitudDto dto)
    {
        var nueva = await servicio.CrearSolicitudAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = nueva.Id }, nueva);
    }

    /// <summary>
    /// Endpoint que ejecuta la Transacción ACID de Adopción
    /// </summary>
    [HttpPut("{id:int}/aprobar")]
    [Authorize(Roles = "Admin,Albergue")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Aprobar(int id)
    {
        var exito = await servicio.AprobarSolicitudAdopcionAsync(id);
        return exito 
            ? Ok(new { mensaje = $"Solicitud {id} aprobada exitosamente bajo transacción ACID. Mascota adoptada." }) 
            : BadRequest(new { mensaje = "No se pudo aprobar la solicitud (verifique estado o disponibilidad)." });
    }

    [HttpPut("{id:int}/rechazar")]
    [Authorize(Roles = "Admin,Albergue")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Rechazar(int id, [FromBody] string? motivo)
    {
        var exito = await servicio.RechazarSolicitudAdopcionAsync(id, motivo ?? "Rechazada por criterio del albergue.");
        return exito 
            ? Ok(new { mensaje = $"Solicitud {id} rechazada." }) 
            : BadRequest(new { mensaje = "No se pudo rechazar la solicitud." });
    }
}
