using AutoMapper;
using BancaLafise.Application.Common.Dtos;
using BancaLafise.Application.Interfaces;
using MediatR;

namespace BancaLafise.Application.Features.Cliente.Commands
{
    public class CrearClienteCommand : IRequest<string>
    {
        public UsuarioDto Usuario { get; set; }
        public ClienteDto Cliente { get; set; }
    }

    public class CrearClienteCommandHandler : IRequestHandler<CrearClienteCommand, string>
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEstadoUsuarioRepository _estadoUsuarioRepository;
        private readonly IMapper _mapper;

        public CrearClienteCommandHandler(IClienteRepository clienteRepository, IUsuarioRepository usuarioRepository, IEstadoUsuarioRepository estadoUsuarioRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _usuarioRepository = usuarioRepository;
            _estadoUsuarioRepository = estadoUsuarioRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(CrearClienteCommand request, CancellationToken cancellationToken) 
        {
            bool existe = await _usuarioRepository.Exist(request.Usuario.Nombre);
            
            if (!existe)
            {
                var cliente = _mapper.Map<Domain.Entities.Cliente>(request.Cliente);

                await _clienteRepository.Create(cliente, cancellationToken);

                var usuario = _mapper.Map<Domain.Entities.Usuario>(request.Usuario);

                usuario.ClienteId = cliente.Id;
                usuario.Clave = await _usuarioRepository.HashClave(usuario.Clave);
                var estados = await _estadoUsuarioRepository.GetAll(cancellationToken);
                usuario.EstadoId = estados.Where(x => x.Descripcion == "ACTIVO").Select(x => x.Id).FirstOrDefault();

                await _usuarioRepository.Create(usuario, cancellationToken);

                return $"Usuario Creado con Éxito ID: {usuario.Id}";
            }
            else
            {
                throw new ArgumentException($"El usuario que intenta crear ya existe {request.Usuario.Nombre}");
            }
        }
    }

}
