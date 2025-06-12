using Moq;
using BancaLafise.Application.Features.Cliente.Commands;
using BancaLafise.Domain.Entities;
using BancaLafise.Application.Interfaces;

public class CreateAccountTests
{
    private readonly Mock<IReglaCumplimientoRepository> _reglasRepoMock;
    private readonly Mock<ITipoCuentaRepository> _tipoCuentaRepoMock;
    private readonly Mock<IClienteRepository> _clienteRepoMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<ICuentaBancariaRepository> _cuentaRepoMock;

    private readonly AgregarCuentaBancariaCommandHandler _handler;

    public CreateAccountTests()
    {
        _reglasRepoMock = new Mock<IReglaCumplimientoRepository>();
        _tipoCuentaRepoMock = new Mock<ITipoCuentaRepository>();
        _clienteRepoMock = new Mock<IClienteRepository>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _cuentaRepoMock = new Mock<ICuentaBancariaRepository>();

        _handler = new AgregarCuentaBancariaCommandHandler(
            _reglasRepoMock.Object,
            _tipoCuentaRepoMock.Object,
            _clienteRepoMock.Object,
            _currentUserServiceMock.Object,
            _cuentaRepoMock.Object
        );
    }

    [Fact]
    public async Task CrearCuentaBancaria_DeberiaCrearCuentaCuandoClienteCumpleReglas()
    {
        // Arrange
        var command = new AgregarCuentaBancariaCommand { TipoCuenta = 1 };

        var tipoCuenta = new TipoCuenta { Id = 1, Descripcion = "Cuenta Ahorro" };
        _tipoCuentaRepoMock.Setup(r => r.Get(1, It.IsAny<CancellationToken>())).ReturnsAsync(tipoCuenta);

        _reglasRepoMock.Setup(r => r.GetAll(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ReglaCumplimiento>
        {
            new() { TipoCuenta = 1, Descripcion = "SALARIO MIN APERTURA", Valor = "1000" },
            new() { TipoCuenta = 1, Descripcion = "MONTO INICIAL", Valor = "500" }
        });

        _currentUserServiceMock.Setup(s => s.ClienteId).Returns("123");

        _clienteRepoMock.Setup(c => c.Get(123, It.IsAny<CancellationToken>())).ReturnsAsync(
            new Cliente { Id = 123, MontoIngreso = 1500 });

        CuentaBancaria? cuentaCreada = null;

        _cuentaRepoMock
            .Setup(c => c.Create(It.IsAny<CuentaBancaria>(), It.IsAny<CancellationToken>()))
            .Callback<CuentaBancaria, CancellationToken>((c, _) => cuentaCreada = c)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("Cuenta de banco creada", result);
        Assert.NotNull(cuentaCreada);
        Assert.Equal(123, cuentaCreada.ClienteId);
        Assert.Equal(500, cuentaCreada.SaldoActual);
        Assert.Equal(1, cuentaCreada.TipoCuenta);
        Assert.False(string.IsNullOrEmpty(cuentaCreada.Numero));
    }
}