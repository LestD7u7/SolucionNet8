using BancaLafise.Application.Common.Dtos;
using BancaLafise.Application.Interfaces;
using MediatR;

namespace BancaLafise.Application.Features.Cliente.Queries
{
    public class ObtenerCuentasBancariasQuery : IRequest<List<CuentaBancariaDto>>
    {
    }

    public class ObtenerCuentasBancariasQueryHandler : IRequestHandler<ObtenerCuentasBancariasQuery, List<CuentaBancariaDto>>
    {
        private readonly ICuentaBancariaRepository _cuentaBancariaRepository;
        private readonly ITipoCuentaRepository _tipoCuentaRepository;

        public ObtenerCuentasBancariasQueryHandler(ICuentaBancariaRepository cuentaBancariaRepository,
            ITipoCuentaRepository tipoCuentaRepository)
        {
            _cuentaBancariaRepository = cuentaBancariaRepository;
            _tipoCuentaRepository = tipoCuentaRepository;
        }

        public async Task<List<CuentaBancariaDto>> Handle(ObtenerCuentasBancariasQuery request, CancellationToken cancellationToken)
        {
            var cuentas = await _cuentaBancariaRepository.GetAll(cancellationToken);

            var tipoCuentas = await _tipoCuentaRepository.GetAll(cancellationToken);

            var response = cuentas.Select(x => new CuentaBancariaDto { 
                Numero = x.Numero, 
                Saldo = x.SaldoActual, 
                TipoCuenta = tipoCuentas.Where(y => y.Id == x.TipoCuenta).Select(y => y.Descripcion).FirstOrDefault()});

            return response.ToList();
        }
    }
}
