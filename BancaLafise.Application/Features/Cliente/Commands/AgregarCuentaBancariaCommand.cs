using BancaLafise.Application.Interfaces;
using BancaLafise.Domain.Entities;
using MediatR;

namespace BancaLafise.Application.Features.Cliente.Commands
{
    public class AgregarCuentaBancariaCommand : IRequest<string>
    {
        public int TipoCuenta { get; set; }
    }

    public class AgregarCuentaBancariaCommandHandler : IRequestHandler<AgregarCuentaBancariaCommand, string>
    {
        private readonly IReglaCumplimientoRepository _reglas;
        private readonly ITipoCuentaRepository _tipoCuentaRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICuentaBancariaRepository _cuentaBancariaRepository;

        public AgregarCuentaBancariaCommandHandler(IReglaCumplimientoRepository reglas,
            ITipoCuentaRepository tipoCuentaRepository,
            IClienteRepository clienteRepository,
            ICurrentUserService currentUserService,
            ICuentaBancariaRepository cuentaBancariaRepository
            )
        {
            _reglas = reglas;
            _tipoCuentaRepository = tipoCuentaRepository;
            _clienteRepository = clienteRepository;
            _currentUserService = currentUserService;
            _cuentaBancariaRepository = cuentaBancariaRepository;
        }

        public async Task<string> Handle(AgregarCuentaBancariaCommand request, CancellationToken cancellationToken)
        {
            var tipoCuenta = await _tipoCuentaRepository.Get(request.TipoCuenta ,cancellationToken);

            if (tipoCuenta is null)
                throw new ArgumentException("El tipo de cuenta que deseas crear no existe");

            var reglas = await _reglas.GetAll(cancellationToken);

            decimal ingresosMin = Convert.ToDecimal(reglas.Where(x => x.TipoCuenta == tipoCuenta.Id && x.Descripcion == "SALARIO MIN APERTURA").Select(x => x.Valor).FirstOrDefault());

            var cliente = await _clienteRepository.Get(Convert.ToInt32(_currentUserService.ClienteId), cancellationToken);

            if (cliente.MontoIngreso >= ingresosMin)
            {
                decimal montoApertura = Convert.ToDecimal(reglas.Where(x => x.TipoCuenta == tipoCuenta.Id && x.Descripcion == "MONTO INICIAL").Select(x => x.Valor).FirstOrDefault());

                string numeroCuenta = $"{cliente.Id}{DateTime.Now.ToString("yyyyMMddHHmmssfff")}";

                var cuentaBank = new CuentaBancaria() { ClienteId = cliente.Id, SaldoActual = montoApertura, TipoCuenta = request.TipoCuenta, Numero = numeroCuenta};

                await _cuentaBancariaRepository.Create(cuentaBank, cancellationToken);

                return $"Cuenta de banco creada No.: {cuentaBank.Numero}, Id: {cuentaBank.Id}";
            }
            else
            {
                throw new ArgumentException($"El cliente no cumple con los ingresos minimos para aperturar la cuenta, ingreso minimo para cuenta {tipoCuenta.Descripcion} es {ingresosMin}");
            }
        }
    }
}
