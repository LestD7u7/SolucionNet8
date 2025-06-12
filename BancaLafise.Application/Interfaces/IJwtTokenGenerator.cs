using BancaLafise.Domain.Entities;

namespace BancaLafise.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Usuario usuario);
    }
}
