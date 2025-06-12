using BancaLafise.Application.Interfaces;
using MediatR;

namespace BancaLafise.Application.Features.Transaccion.Commands
{
    public class AplicarInteresCompuestoCommand : IRequest<string>
    {
        public string NumeroCuenta { get; set; }
        public decimal PorcentajeInteres { get; set; }
    }

    public class AplicarInteresCompuestoCommandHandler : IRequestHandler<AplicarInteresCompuestoCommand, string>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICuentaBancariaRepository _cuentaBancariaRepository;
        private readonly ITipoTransaccionRepository _tipoTransaccionRepository;

        public AplicarInteresCompuestoCommandHandler(ITransactionRepository transactionRepository,
            ICuentaBancariaRepository cuentaBancariaRepository,
            ITipoTransaccionRepository tipoTransaccionRepository
            )
        {
            _transactionRepository = transactionRepository;
            _cuentaBancariaRepository = cuentaBancariaRepository;
            _tipoTransaccionRepository = tipoTransaccionRepository;
        }

        public async Task<string> Handle(AplicarInteresCompuestoCommand request, CancellationToken cancellationToken)
        {
            if (request.PorcentajeInteres < 0)
                throw new ArgumentException("El interés no puede ser negativo.");

            if (request.PorcentajeInteres > 100)
                throw new ArgumentException("El interés no puede ser mayor al 100%.");

            var cuenta = await _cuentaBancariaRepository.GetByNumber(request.NumeroCuenta);

            if (cuenta == null)
                throw new ArgumentException("Cuenta no encontrada");

            decimal tasa = request.PorcentajeInteres / 100m;
            var saldoPrevio = cuenta.SaldoActual;
            cuenta.SaldoActual *= (1 + tasa);

            await _cuentaBancariaRepository.Update(cuenta, cancellationToken);

            var tiposTran = await _tipoTransaccionRepository.GetAll(cancellationToken);

            string numeroRef = $"{cuenta.Id}{DateTime.Now.ToString("yyyyMMddHHmmssfff")}";

            var transaccion = new BancaLafise.Domain.Entities.Transaccion()
            {
                NumeroReferencia = numeroRef,
                Monto = cuenta.SaldoActual - saldoPrevio,
                CuentaDestino = cuenta.Id,
                SaldoDestino = cuenta.SaldoActual,
                TipoTransaccion = tiposTran.FirstOrDefault(x => x.Descripcion == "APLICACION DE INTERES COMPUESTO").Id
            };

            await _transactionRepository.Create(transaccion, cancellationToken);

            return $"APLICACION DE INTERES COMPUESTO REALIZADO CON EXITO REF: {numeroRef}, ID: {transaccion.Id}, SALDO ACTUAL: {transaccion.SaldoDestino}" +
                $", INTERES APLICADO %{request.PorcentajeInteres}";
        }
    }
}
