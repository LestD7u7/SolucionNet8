using BancaLafise.Application.Common.Dtos;
using BancaLafise.Application.Features.Transaccion.Commands;
using BancaLafise.Application.Interfaces;
using MediatR;

namespace BancaLafise.Application.Features.Transaccion.Queries
{
    public class HistorialTransaccionesCuentaQuery : IRequest<HistorialTransaccionesResponseDto>
    {
        public string NumeroCuenta { get; set; }
    }

    public class HistorialTransaccionesCuentaQueryHandler : IRequestHandler<HistorialTransaccionesCuentaQuery, HistorialTransaccionesResponseDto>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICuentaBancariaRepository _cuentaBancariaRepository;
        private readonly ITipoTransaccionRepository _tipoTransaccionRepository;
        private readonly ITipoCuentaRepository _tipoCuentaRepository;
        private readonly ITipoMovimientoRepository _tipoMovimientoRepository;

        public HistorialTransaccionesCuentaQueryHandler(ITransactionRepository transactionRepository,
            ICuentaBancariaRepository cuentaBancariaRepository,
            ITipoTransaccionRepository tipoTransaccionRepository,
            ITipoCuentaRepository tipoCuentaRepository,
            ITipoMovimientoRepository tipoMovimientoRepository
            )
        {
            _transactionRepository = transactionRepository;
            _cuentaBancariaRepository = cuentaBancariaRepository;
            _tipoTransaccionRepository = tipoTransaccionRepository;
            _tipoCuentaRepository = tipoCuentaRepository;
            _tipoMovimientoRepository = tipoMovimientoRepository;
        }

        public async Task<HistorialTransaccionesResponseDto> Handle(HistorialTransaccionesCuentaQuery request, CancellationToken cancellationToken)
        {
            var cuenta = await _cuentaBancariaRepository.GetByNumber(request.NumeroCuenta);

            if (cuenta == null)
                throw new ArgumentException("Cuenta origen no encontrada");

            var transacciones = await _transactionRepository.GetAllbyCuenta(cuenta.Id, cancellationToken);

            var tipoCuentas = await _tipoCuentaRepository.GetAll(cancellationToken);
            var tipoMovimientos = await _tipoMovimientoRepository.GetAll(cancellationToken);
            var tipoTransacciones = await _tipoTransaccionRepository.GetAll(cancellationToken);

            var transaccionesDto = transacciones.Select(x => new TransaccionDto()
            {
                Referencia = x.NumeroReferencia, 
                Monto = $"{(cuenta.Id == x.CuentaOrigen ? "-" : "+")}{x.Monto}", 
                Saldo = (cuenta.Id == x.CuentaOrigen ? x.SaldoOrigen.ToString() : x.SaldoDestino.ToString()), 
                TipoMovimiento = (cuenta.Id == x.CuentaOrigen ? "DEBITO" : "CREDITO"),
                TipoTransaccion = tipoTransacciones.FirstOrDefault(y => y.Id == x.TipoTransaccion).Descripcion,
                Fecha = x.FechaRegistro.ToString(),
            });

            var response = new HistorialTransaccionesResponseDto
            {
                Cuenta = new CuentaBancariaDto
                {
                    Numero = cuenta.Numero,
                    Saldo = cuenta.SaldoActual,
                    TipoCuenta = tipoCuentas.Where(x => x.Id == cuenta.TipoCuenta).Select(x => x.Descripcion).FirstOrDefault()
                },
                Transacciones = transaccionesDto.ToList()
            };

            return response;
        }
    }
}
