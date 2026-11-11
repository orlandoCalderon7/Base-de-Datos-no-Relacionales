namespace MascotasService.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MascotasService.Application.DTOs;
using MascotasService.Domain.Entities;
using MascotasService.Infrastructure.Data;

[ApiController]
[Route("api/[controller]")]
public class AlberguesController(AdoptaMasDbContext context) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AlbergueRespuestaDto>))]
    public async Task<IActionResult> ObtenerTodos()
    {
        var albergues = await context.Albergues
            .Include(a => a.Mascotas)
            .AsNoTracking()
            .ToListAsync();

        var dtos = albergues.Select(a => new AlbergueRespuestaDto(
            a.Id, a.Nombre, a.Direccion, a.Ciudad, a.Telefono, a.EmailContacto, a.CapacidadMaxima, a.EsActivo, a.Mascotas.Count
        )).ToList();

        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AlbergueRespuestaDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var a = await context.Albergues
            .Include(a => a.Mascotas)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (a is null) return NotFound(new { mensaje = $"Albergue {id} no encontrado." });

        return Ok(new AlbergueRespuestaDto(
            a.Id, a.Nombre, a.Direccion, a.Ciudad, a.Telefono, a.EmailContacto, a.CapacidadMaxima, a.EsActivo, a.Mascotas.Count
        ));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AlbergueRespuestaDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear([FromBody] CrearAlbergueDto dto)
    {
        var albergue = new Albergue
        {
            Nombre = dto.Nombre,
            Direccion = dto.Direccion,
            Ciudad = dto.Ciudad,
            Telefono = dto.Telefono,
            EmailContacto = dto.EmailContacto,
            CapacidadMaxima = dto.CapacidadMaxima,
            EsActivo = true
        };

        context.Albergues.Add(albergue);
        await context.SaveChangesAsync();

        var respuesta = new AlbergueRespuestaDto(
            albergue.Id, albergue.Nombre, albergue.Direccion, albergue.Ciudad, albergue.Telefono, albergue.EmailContacto, albergue.CapacidadMaxima, albergue.EsActivo, 0
        );

        return CreatedAtAction(nameof(ObtenerPorId), new { id = albergue.Id }, respuesta);
    }
}
