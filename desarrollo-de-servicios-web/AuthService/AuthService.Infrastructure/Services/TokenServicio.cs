namespace AuthService.Infrastructure.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;

public class TokenServicio(IConfiguration config) : ITokenServicio
{
    public string GenerarToken(Usuario usuario, out DateTime expiraEnUtc)
    {
        var secretKey = config["Jwt:SecretKey"] ?? "ClaveSecretaSuperSeguraZegelDesarrolloServiciosWeb2026!";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Name, usuario.NombreCompleto),
            new(ClaimTypes.Email, usuario.Email),
            new(ClaimTypes.Role, usuario.Rol)
        };

        if (usuario.AlbergueId.HasValue)
        {
            claims.Add(new Claim("AlbergueId", usuario.AlbergueId.Value.ToString()));
        }

        expiraEnUtc = DateTime.UtcNow.AddHours(8);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiraEnUtc,
            Issuer = config["Jwt:Issuer"] ?? "ZegelAdoptaMasAPI",
            Audience = config["Jwt:Audience"] ?? "ZegelEstudiantes",
            SigningCredentials = credenciales
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
