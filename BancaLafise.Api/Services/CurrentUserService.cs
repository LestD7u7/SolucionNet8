using BancaLafise.Application.Interfaces;
using BancaLafise.Infrastructure.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BancaLafise.Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string ClienteId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimNames.ClienteId)!;

        public string UsuarioId => _httpContextAccessor.HttpContext?.User?.Claims.Where(x => x.Properties.Any(z => z.Value == ClaimTypes.NameIdentifier)).FirstOrDefault().Value;

        public string Username => _httpContextAccessor.HttpContext?.User?.Claims.Where(x => x.Properties.Any(z => z.Value == ClaimTypes.Name)).FirstOrDefault().Value;
    }
}
