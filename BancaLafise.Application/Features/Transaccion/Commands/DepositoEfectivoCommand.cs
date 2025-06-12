using BancaLafise.Application.Interfaces;
using MediatR;

namespace BancaLafise.Application.Features.Transaccion.Commands
{
    public class DepositoEfectivoCommand : IRequest<string>
    {
        public string NumeroCuentaDestino { get; set; }
        public decimal Monto { get; set; }
    }

    public class DepositoEfectivoCommandHandler : IRequestHandler<DepositoEfectivoCommand, string>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICuentaBancariaRepository _cuentaBancariaRepository;
        private readonly ITipoTransaccionRepository _tipoTransaccionRepository;

        public DepositoEfectivoCommandHandler(ITransactionRepository transactionRepository,
            ICuentaBancariaRepository cuentaBancariaRepository,
            ITipoTransaccionRepository tipoTransaccionRepository
            )
        {
            _transactionRepository = transactionRepository;
            _cuentaBancariaRepository = cuentaBancariaRepository;
            _tipoTransaccionRepository = tipoTransaccionRepository;
        }

        public async Task<string> Handle(DepositoEfectivoCommand request, CancellationToken cancellationToken)
        {
            var cuenta = await _cuentaBancariaRepository.GetByNumber(request.NumeroCuentaDestino);

            if (cuenta == null)
                throw new ArgumentException("Cuenta no encontrada");

            cuenta.SaldoActual += request.Monto;

            await _cuentaBancariaRepository.Update(cuenta, cancellationToken);

            var tiposTran = await _tipoTransaccionRepository.GetAll(cancellationToken);

            string numeroRef = $"{cuenta.Id}{DateTime.Now.ToString("yyyyMMddHHmmssfff")}";

            var transaccion = new BancaLafise.Domain.Entities.Transaccion()
            {
                NumeroReferencia = numeroRef,
                Monto = request.Monto,
                CuentaDestino = cuenta.Id,
                SaldoDestino = cuenta.SaldoActual,
                TipoTransaccion = tiposTran.FirstOrDefault(x => x.Descripcion == "DEPOSITO EN EFECTIVO").Id
            };

            await _transactionRepository.Create(transaccion, cancellationToken);

            return $"DEPOSITO EN EFECTIVO REALIZADO CON EXITO REF: {numeroRef}, ID: {transaccion.Id}, SALDO ACTUAL: {transaccion.SaldoDestino}";
        }
    }
}
