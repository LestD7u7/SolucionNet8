using BancaLafise.Application.Common.Dtos;
using BancaLafise.Application.Interfaces;
using MediatR;

namespace BancaLafise.Application.Features.Cliente.Commands
{
    public class InicioSesionCommand : IRequest<TokenResponseDto>
    {
        public string Usuario { get; set; }
        public string Clave { get; set; }
    }

    public class InicioSesionCommandHandler : IRequestHandler<InicioSesionCommand, TokenResponseDto>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public InicioSesionCommandHandler(IUsuarioRepository usuarioRepository, IJwtTokenGenerator jwtToken)
        {
            _usuarioRepository = usuarioRepository;
            _jwtTokenGenerator = jwtToken;
        }

        public async Task<TokenResponseDto> Handle(InicioSesionCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.GetUserName(request.Usuario);

            if (usuario == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            bool valido = await _usuarioRepository.ValidarClave(usuario, request.Clave);

            if (!valido)
                throw new UnauthorizedAccessException("Invalid credentials");

            var token = _jwtTokenGenerator.GenerateToken(usuario);

            return new() { AccessToken= token, TokenType= "bearer" };
        }
    }
}
