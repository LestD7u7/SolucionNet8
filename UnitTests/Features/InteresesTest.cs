using Moq;
using BancaLafise.Application.Features.Transaccion.Commands;
using BancaLafise.Application.Interfaces;
using BancaLafise.Domain.Entities;

public class InterestTests
{
    private readonly Mock<ITransactionRepository> _transactionRepoMock;
    private readonly Mock<ICuentaBancariaRepository> _cuentaRepoMock;
    private readonly Mock<ITipoTransaccionRepository> _tipoTransaccionRepoMock;
    private readonly AplicarInteresCompuestoCommandHandler _handler;

    public InterestTests()
    {
        _transactionRepoMock = new Mock<ITransactionRepository>();
        _cuentaRepoMock = new Mock<ICuentaBancariaRepository>();
        _tipoTransaccionRepoMock = new Mock<ITipoTransaccionRepository>();

        _handler = new AplicarInteresCompuestoCommandHandler(
            _transactionRepoMock.Object,
            _cuentaRepoMock.Object,
            _tipoTransaccionRepoMock.Object
        );
    }

    [Fact]
    public async Task AplicarInteres_DeberiaActualizarSaldoYRegistrarTransaccion()
    {
        // Arrange
        var cuenta = new CuentaBancaria
        {
            Id = 1,
            Numero = "123456",
            SaldoActual = 1000m
        };

        var command = new AplicarInteresCompuestoCommand
        {
            NumeroCuenta = "123456",
            PorcentajeInteres = 10m
        };

        _cuentaRepoMock.Setup(r => r.GetByNumber("123456"))
                       .ReturnsAsync(cuenta);

        _tipoTransaccionRepoMock.Setup(r => r.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TipoTransaccion>
            {
                new() { Id = 3, Descripcion = "APLICACION DE INTERES COMPUESTO" }
            });

        BancaLafise.Domain.Entities.Transaccion? transaccion = null;
        _transactionRepoMock.Setup(t => t.Create(It.IsAny<BancaLafise.Domain.Entities.Transaccion>(), It.IsAny<CancellationToken>()))
            .Callback<BancaLafise.Domain.Entities.Transaccion, CancellationToken>((t, _) => transaccion = t)
            .Returns(Task.CompletedTask);

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Contains("APLICACION DE INTERES COMPUESTO REALIZADO CON EXITO", resultado);
        Assert.Equal(1100m, cuenta.SaldoActual);
        Assert.NotNull(transaccion);
        Assert.Equal(100m, transaccion.Monto);
        Assert.Equal(3, transaccion.TipoTransaccion);
    }

    [Fact]
    public async Task AplicarInteres_DeberiaLanzarError_SiCuentaNoExiste()
    {
        // Arrange
        var command = new AplicarInteresCompuestoCommand
        {
            NumeroCuenta = "000000",
            PorcentajeInteres = 5m
        };

        _cuentaRepoMock.Setup(r => r.GetByNumber("000000"))
                       .ReturnsAsync((CuentaBancaria?)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Cuenta no encontrada", ex.Message);
    }

    [Fact]
    public async Task AplicarInteres_DeberiaLanzarError_SiInteresNegativo()
    {
        // Arrange
        var command = new AplicarInteresCompuestoCommand
        {
            NumeroCuenta = "123456",
            PorcentajeInteres = -1m
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("El interés no puede ser negativo.", ex.Message);
    }

    [Fact]
    public async Task AplicarInteres_DeberiaLanzarError_SiInteresMayorA100()
    {
        // Arrange
        var command = new AplicarInteresCompuestoCommand
        {
            NumeroCuenta = "123456",
            PorcentajeInteres = 150m
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("El interés no puede ser mayor al 100%.", ex.Message);
    }
}
