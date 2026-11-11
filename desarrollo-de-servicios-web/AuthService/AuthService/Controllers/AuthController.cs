namespace AuthService.Controllers;

using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ITokenServicio tokenServicio) : ControllerBase
{
    private static readonly List<Usuario> _usuarios =
    [
        new Usuario
        {
            Id = 1,
            NombreCompleto = "Administrador General",
            Email = "admin@zegel.pe",
            PasswordHash = HashPassword("Admin123!"),
            Rol = "Admin"
        },
        new Usuario
        {
            Id = 2,
            NombreCompleto = "Albergue Patitas",
            Email = "patitas@adopta.pe",
            PasswordHash = HashPassword("Albergue123!"),
            Rol = "Albergue",
            AlbergueId = 1
        },
        new Usuario
        {
            Id = 3,
            NombreCompleto = "Juan Pérez (Adoptante)",
            Email = "juan@gmail.com",
            PasswordHash = HashPassword("Juan123!"),
            Rol = "Adoptante"
        }
    ];
    private static int _siguienteId = 4;

    [HttpPost("registro")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AuthRespuestaDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Registro([FromBody] RegistroDto dto)
    {
        if (_usuarios.Any(u => u.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase)))
        {
            return BadRequest(new { mensaje = "El correo electrónico ya se encuentra registrado." });
        }

        var nuevo = new Usuario
        {
            Id = _siguienteId++,
            NombreCompleto = dto.NombreCompleto,
            Email = dto.Email,
            PasswordHash = HashPassword(dto.Password),
            Telefono = dto.Telefono,
            Rol = dto.Rol,
            AlbergueId = dto.AlbergueId,
            FechaRegistro = DateTime.UtcNow
        };

        _usuarios.Add(nuevo);

        var token = tokenServicio.GenerarToken(nuevo, out var expira);
        var respuesta = new AuthRespuestaDto(nuevo.Id, nuevo.NombreCompleto, nuevo.Email, nuevo.Rol, nuevo.AlbergueId, token, expira);

        return CreatedAtAction(nameof(Registro), new { id = nuevo.Id }, respuesta);
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthRespuestaDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var usuario = _usuarios.FirstOrDefault(u => u.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase));
        if (usuario is null || usuario.PasswordHash != HashPassword(dto.Password))
        {
            return Unauthorized(new { mensaje = "Credenciales inválidas (correo o contraseña incorrectos)." });
        }

        var token = tokenServicio.GenerarToken(usuario, out var expira);
        return Ok(new AuthRespuestaDto(usuario.Id, usuario.NombreCompleto, usuario.Email, usuario.Rol, usuario.AlbergueId, token, expira));
    }

    [HttpGet("usuarios")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult ListarUsuarios()
    {
        var resumen = _usuarios.Select(u => new
        {
            u.Id,
            u.NombreCompleto,
            u.Email,
            u.Rol,
            u.AlbergueId,
            u.FechaRegistro
        });
        return Ok(resumen);
    }

    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
