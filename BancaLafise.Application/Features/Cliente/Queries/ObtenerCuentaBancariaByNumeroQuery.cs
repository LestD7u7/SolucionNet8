using BancaLafise.Application.Common.Dtos;
using BancaLafise.Application.Interfaces;
using MediatR;

namespace BancaLafise.Application.Features.Cliente.Queries
{
    public class ObtenerCuentaBancariaByNumeroQuery : IRequest<CuentasBancariasResponseDto>
    {
        public string Numero { get; set; }
    }

    public class ObtenerCuentaBancariaByNumeroQueryHandler : IRequestHandler<ObtenerCuentaBancariaByNumeroQuery, CuentasBancariasResponseDto>
    {
        private readonly ICuentaBancariaRepository _cuentaBancariaRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly ITipoCuentaRepository _tipoCuentaRepository;

        public ObtenerCuentaBancariaByNumeroQueryHandler(ICuentaBancariaRepository cuentaBancariaRepository, 
            IClienteRepository clienteRepository,
            ITipoCuentaRepository tipoCuentaRepository)
        {
            _cuentaBancariaRepository = cuentaBancariaRepository;
            _clienteRepository = clienteRepository;
            _tipoCuentaRepository = tipoCuentaRepository;
        }

        public async Task<CuentasBancariasResponseDto> Handle(ObtenerCuentaBancariaByNumeroQuery request, CancellationToken cancellationToken)
        {
            var cuenta = await _cuentaBancariaRepository.GetByNumber(request.Numero);

            if (cuenta == null)
                throw new InvalidDataException("Cuenta no encontrada");

            var tipoCuenta = await _tipoCuentaRepository.Get(cuenta.TipoCuenta, cancellationToken);

            var cliente = await _clienteRepository.Get(cuenta.ClienteId, cancellationToken);

            return new() {
                cliente = new ClienteDto 
                {
                    Nombre = cliente.Nombre,
                    Apellido = cliente.Apellido,
                    FechaNacimiento = cliente.FechaNacimiento,
                    MontoIngreso = cliente.MontoIngreso,
                    SexoId = cliente.SexoId
                },
                cuentas = new List<CuentaBancariaDto>
                {
                    new CuentaBancariaDto { Numero = cuenta.Numero, Saldo = cuenta.SaldoActual, TipoCuenta = tipoCuenta.Descripcion}
                }
            };
        }
    }
}
