using BancaLafise.Application.Common.Dtos;
using BancaLafise.Application.Interfaces;
using BancaLafise.Domain.Entities;
using MediatR;

namespace BancaLafise.Application.Features.AdminCatalogos
{
    public class ConsultarCatalogosBaseQuery : IRequest<List<CatalogosResponseDto>>
    {

    }

    public class ConsultarCatalogosBaseQueryHandler : IRequestHandler<ConsultarCatalogosBaseQuery, List<CatalogosResponseDto>>
    {
        private readonly ISexoRepository _sexoRepository;
        private readonly ITipoCuentaRepository _tipoCuentaRepository;
        private readonly IEstadoUsuarioRepository _estadoUsuarioRepository;
        private readonly ITipoMovimientoRepository _tipoMovimientoRepository;
        private readonly ITipoTransaccionRepository _tipoTransaccionRepository;
        private readonly IReglaCumplimientoRepository _reglaCumplimientoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICuentaBancariaRepository _cuentaBancariaRepository;

        public ConsultarCatalogosBaseQueryHandler(ISexoRepository sexoRepository,
            ITipoCuentaRepository tipoCuentaRepository,
            IEstadoUsuarioRepository estadoUsuarioRepository,
            ITipoMovimientoRepository tipoMovimientoRepository,
            ITipoTransaccionRepository tipoTransaccionRepository,
            IReglaCumplimientoRepository reglaCumplimientoRepository,
            IUsuarioRepository usuarioRepository,
            IClienteRepository clienteRepository,
            ITransactionRepository transactionRepository,
            ICuentaBancariaRepository cuentaBancariaRepository
            )
        {
            _sexoRepository = sexoRepository;
            _tipoCuentaRepository = tipoCuentaRepository;
            _estadoUsuarioRepository = estadoUsuarioRepository;
            _tipoMovimientoRepository = tipoMovimientoRepository;
            _tipoTransaccionRepository = tipoTransaccionRepository;
            _reglaCumplimientoRepository = reglaCumplimientoRepository;
            _usuarioRepository = usuarioRepository;
            _clienteRepository = clienteRepository;
            _transactionRepository = transactionRepository;
            _cuentaBancariaRepository = cuentaBancariaRepository;
        }

        public async Task<List<CatalogosResponseDto>> Handle(ConsultarCatalogosBaseQuery request, CancellationToken cancellationToken)
        {
            var sexos = await _sexoRepository.GetAll(cancellationToken);
            var tiposCuenta = await _tipoCuentaRepository.GetAll(cancellationToken);
            var estadosUsuario = await _estadoUsuarioRepository.GetAll(cancellationToken);
            var tiposMovimiento = await _tipoMovimientoRepository.GetAll(cancellationToken);
            var tiposTransaccion = await _tipoTransaccionRepository.GetAll(cancellationToken);
            var reglasCumplimiento = await _reglaCumplimientoRepository.GetAll(cancellationToken);
            var usuarios = await _usuarioRepository.GetAll(cancellationToken);
            var clientes = await _clienteRepository.GetAll(cancellationToken);
            var cuentas = await _cuentaBancariaRepository.GetAll(cancellationToken);
            var transacciones = await _transactionRepository.GetAll(cancellationToken);

            return new() {
                new CatalogosResponseDto { NombreTabla = "Sexos", Registros = new List<object> { sexos } },
                new CatalogosResponseDto { NombreTabla = "TiposCuentas", Registros = new List<object> { tiposCuenta } },
                new CatalogosResponseDto { NombreTabla = "EstadosUsuarios", Registros = new List<object> { estadosUsuario } },
                new CatalogosResponseDto { NombreTabla = "TiposMovimientos", Registros = new List<object> { tiposMovimiento } },
                new CatalogosResponseDto { NombreTabla = "TiposTransacciones", Registros = new List<object> { tiposTransaccion } },
                new CatalogosResponseDto { NombreTabla = "ReglasCumplimiento", Registros = new List<object> { reglasCumplimiento } },
                new CatalogosResponseDto { NombreTabla = "Usuarios", Registros = new List<object> { usuarios } },
                new CatalogosResponseDto { NombreTabla = "Clientes", Registros = new List<object> { clientes } },
                new CatalogosResponseDto { NombreTabla = "Cuentas", Registros = new List<object> { cuentas } },
                new CatalogosResponseDto { NombreTabla = "Transacciones", Registros = new List<object> { transacciones } }
            };
        }
    }
}
