using BancaLafise.Application.Interfaces;
using MediatR;

namespace BancaLafise.Application.Features.Transaccion.Commands
{
    public class RetiroEfectivoCommand : IRequest<string>
    {
        public string NumeroCuentaOrigen { get; set; }
        public decimal Monto { get; set; }
    }
    public class RetiroEfectivoCommandHandler : IRequestHandler<RetiroEfectivoCommand, string>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICuentaBancariaRepository _cuentaBancariaRepository;
        private readonly ITipoTransaccionRepository _tipoTransaccionRepository;
        private readonly ICurrentUserService _currentUserService;

        public RetiroEfectivoCommandHandler(ITransactionRepository transactionRepository,
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

        public async Task<string> Handle(RetiroEfectivoCommand request, CancellationToken cancellationToken)
        {
            var cuenta = await _cuentaBancariaRepository.GetByNumber(request.NumeroCuentaOrigen);

            if (cuenta == null)
                throw new ArgumentException("Cuenta no encontrada");

            bool isCuentaPropia = await _cuentaBancariaRepository.Valid(cuenta.Numero, Convert.ToInt32(_currentUserService.ClienteId));

            if (!isCuentaPropia)
                throw new UnauthorizedAccessException("Cuenta origen no pertece al Cliente");

            bool SaldoDisponible = await _transactionRepository.validMontoDebito(cuenta, request.Monto);

            if (!SaldoDisponible)
                throw new ArgumentException("Saldo Insuficiente para completar la operación");

            cuenta.SaldoActual -= request.Monto;

            await _cuentaBancariaRepository.Update(cuenta, cancellationToken);

            var tiposTran = await _tipoTransaccionRepository.GetAll(cancellationToken);

            string numeroRef = $"{cuenta.Id}{DateTime.Now.ToString("yyyyMMddHHmmssfff")}";

            var transaccion = new BancaLafise.Domain.Entities.Transaccion()
            {
                NumeroReferencia = numeroRef,
                Monto = request.Monto,
                CuentaOrigen = cuenta.Id,
                SaldoOrigen = cuenta.SaldoActual,
                TipoTransaccion = tiposTran.FirstOrDefault(x => x.Descripcion == "RETIRO EN EFECTIVO").Id
            };

            await _transactionRepository.Create(transaccion, cancellationToken);

            return $"RETIRO EN EFECTIVO REALIZADO CON EXITO REF: {numeroRef}, ID: {transaccion.Id}, SALDO ACTUAL: {transaccion.SaldoDestino}";
        }
    }
}
