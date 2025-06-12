using Moq;
using BancaLafise.Domain.Entities;
using BancaLafise.Application.Interfaces;
using BancaLafise.Application.Features.Transaccion.Commands;

public class RetiroEfectivoTest
{
    private readonly Mock<ITransactionRepository> _transactionRepoMock;
    private readonly Mock<ICuentaBancariaRepository> _cuentaRepoMock;
    private readonly Mock<ITipoTransaccionRepository> _tipoTransaccionRepoMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;

    private readonly RetiroEfectivoCommandHandler _handler;

    public RetiroEfectivoTest()
    {
        _transactionRepoMock = new Mock<ITransactionRepository>();
        _cuentaRepoMock = new Mock<ICuentaBancariaRepository>();
        _tipoTransaccionRepoMock = new Mock<ITipoTransaccionRepository>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();

        _handler = new RetiroEfectivoCommandHandler(
            _transactionRepoMock.Object,
            _cuentaRepoMock.Object,
            _tipoTransaccionRepoMock.Object,
            _currentUserServiceMock.Object
        );
    }

    [Fact]
    public async Task RetiroEfectivo_DeberiaActualizarSaldoYRegistrarTransaccion()
    {
        // Arrange
        var cuenta = new CuentaBancaria
        {
            Id = 1,
            Numero = "123456",
            SaldoActual = 1000m
        };

        var command = new RetiroEfectivoCommand
        {
            NumeroCuentaOrigen = "123456",
            Monto = 300m
        };

        _currentUserServiceMock.Setup(c => c.ClienteId).Returns("1");

        _cuentaRepoMock.Setup(r => r.GetByNumber("123456"))
                       .ReturnsAsync(cuenta);

        _cuentaRepoMock.Setup(r => r.Valid("123456", 1))
                       .ReturnsAsync(true);

        _transactionRepoMock.Setup(t => t.validMontoDebito(cuenta, 300m))
                            .ReturnsAsync(true);

        _tipoTransaccionRepoMock.Setup(r => r.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TipoTransaccion>
            {
                new() { Id = 2, Descripcion = "RETIRO EN EFECTIVO" }
            });

        BancaLafise.Domain.Entities.Transaccion? transaccion = null;
        _transactionRepoMock.Setup(t => t.Create(It.IsAny<BancaLafise.Domain.Entities.Transaccion>(), It.IsAny<CancellationToken>()))
            .Callback<BancaLafise.Domain.Entities.Transaccion, CancellationToken>((t, _) => transaccion = t)
            .Returns(Task.CompletedTask);

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Contains("RETIRO EN EFECTIVO REALIZADO CON EXITO", resultado);
        Assert.Equal(700m, cuenta.SaldoActual);
        Assert.NotNull(transaccion);
        Assert.Equal(2, transaccion.TipoTransaccion);
        Assert.Equal(700m, transaccion.SaldoOrigen);
    }

    [Fact]
    public async Task RetiroEfectivo_DeberiaLanzarError_SiCuentaNoExiste()
    {
        // Arrange
        var command = new RetiroEfectivoCommand
        {
            NumeroCuentaOrigen = "999999",
            Monto = 100
        };

        _cuentaRepoMock.Setup(r => r.GetByNumber("999999"))
                       .ReturnsAsync((CuentaBancaria?)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Cuenta no encontrada", ex.Message);
    }

    [Fact]
    public async Task RetiroEfectivo_DeberiaLanzarError_SiCuentaNoEsDelCliente()
    {
        // Arrange
        var cuenta = new CuentaBancaria { Id = 1, Numero = "123456", SaldoActual = 1000m };

        var command = new RetiroEfectivoCommand
        {
            NumeroCuentaOrigen = "123456",
            Monto = 100
        };

        _currentUserServiceMock.Setup(c => c.ClienteId).Returns("1");

        _cuentaRepoMock.Setup(r => r.GetByNumber("123456")).ReturnsAsync(cuenta);
        _cuentaRepoMock.Setup(r => r.Valid("123456", 1)).ReturnsAsync(false);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Cuenta origen no pertece al Cliente", ex.Message);
    }

    [Fact]
    public async Task RetiroEfectivo_DeberiaLanzarError_SiSaldoInsuficiente()
    {
        // Arrange
        var cuenta = new CuentaBancaria { Id = 1, Numero = "123456", SaldoActual = 100m };

        var command = new RetiroEfectivoCommand
        {
            NumeroCuentaOrigen = "123456",
            Monto = 200m
        };

        _currentUserServiceMock.Setup(c => c.ClienteId).Returns("1");

        _cuentaRepoMock.Setup(r => r.GetByNumber("123456")).ReturnsAsync(cuenta);
        _cuentaRepoMock.Setup(r => r.Valid("123456", 1)).ReturnsAsync(true);
        _transactionRepoMock.Setup(r => r.validMontoDebito(cuenta, 200m)).ReturnsAsync(false);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Saldo Insuficiente para completar la operación", ex.Message);
    }
}
