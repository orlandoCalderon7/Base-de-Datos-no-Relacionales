namespace AuthService.Application.Interfaces;

using AuthService.Domain.Entities;

public interface ITokenServicio
{
    string GenerarToken(Usuario usuario, out DateTime expiraEnUtc);
}
