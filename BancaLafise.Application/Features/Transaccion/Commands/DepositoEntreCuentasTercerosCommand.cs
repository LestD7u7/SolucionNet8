using BancaLafise.Application.Interfaces;
using MediatR;

namespace BancaLafise.Application.Features.Transaccion.Commands
{
    public class DepositoEntreCuentasTercerosCommand : IRequest<string>
    {
        public string NumeroCuentaOrigen { get; set; }
        public string NumeroCuentaDestino { get; set; }
        public decimal Monto { get; set; }
    }

    public class DepositoEntreCuentasTercerosCommandHandler : IRequestHandler<DepositoEntreCuentasTercerosCommand, string>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICuentaBancariaRepository _cuentaBancariaRepository;
        private readonly ITipoTransaccionRepository _tipoTransaccionRepository;
        private readonly ICurrentUserService _currentUserService;

        public DepositoEntreCuentasTercerosCommandHandler(ITransactionRepository transactionRepository,
            ICuentaBancariaRepository cuentaBancariaRepository,
            ITipoTransaccionRepository tipoTransaccionRepository,
            ICurrentUserService currentUserService
            )
        {
            _transactionRepository = transactionRepository;
            _cuentaBancariaRepository = cuentaBancariaRepository;
            _tipoTransaccionRepository = tipoTransaccionRepository;
            _currentUserService = currentUserService;
        }

        public async Task<string> Handle(DepositoEntreCuentasTercerosCommand request, CancellationToken cancellationToken)
        {
            var cuentaOrigen = await _cuentaBancariaRepository.GetByNumber(request.NumeroCuentaOrigen);

            if (cuentaOrigen == null)
                throw new ArgumentException("Cuenta origen no encontrada");

            bool isCuentaPropia = await _cuentaBancariaRepository.Valid(cuentaOrigen.Numero, Convert.ToInt32(_currentUserService.ClienteId));

            if (!isCuentaPropia)
                throw new UnauthorizedAccessException("Cuenta origen no pertece al Cliente");

            bool SaldoDisponible = await _transactionRepository.validMontoDebito(cuentaOrigen, request.Monto);

            if (!SaldoDisponible)
                throw new ArgumentException("Saldo Insuficiente para completar la operación");

            var cuentaDestino = await _cuentaBancariaRepository.GetByNumber(request.NumeroCuentaDestino);

            if (cuentaDestino == null)
                throw new ArgumentException("Cuenta destino no encontrada");

            cuentaOrigen.SaldoActual -= request.Monto;

            await _cuentaBancariaRepository.Update(cuentaOrigen, cancellationToken);

            cuentaDestino.SaldoActual += request.Monto;

            await _cuentaBancariaRepository.Update(cuentaDestino, cancellationToken);

            var tiposTran = await _tipoTransaccionRepository.GetAll(cancellationToken);

            string numeroRef = $"{cuentaOrigen.Id}{DateTime.Now.ToString("yyyyMMddHHmmssfff")}";

            var transaccion = new BancaLafise.Domain.Entities.Transaccion()
            {
                NumeroReferencia = numeroRef,
                Monto = request.Monto,
                CuentaOrigen = cuentaOrigen.Id,
                SaldoOrigen = cuentaOrigen.SaldoActual,
                CuentaDestino = cuentaDestino.Id,
                SaldoDestino = cuentaDestino.SaldoActual,
                TipoTransaccion = tiposTran.FirstOrDefault(x => x.Descripcion == "DEPOSITO TERCERO").Id
            };

            await _transactionRepository.Create(transaccion, cancellationToken);

            return $"DEPOSITO TERCERO REALIZADO CON EXITO REF: {numeroRef}, ID: {transaccion.Id}, MI SALDO ACTUAL: {transaccion.SaldoOrigen} " +
                $"CUENTA ORIGEN: {cuentaOrigen.Numero} CUENTA DESTINO: {cuentaDestino.Numero}";
        }
    }
}
