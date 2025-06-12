using Moq;
using BancaLafise.Application.Features.Transaccion.Commands;
using BancaLafise.Application.Interfaces;
using BancaLafise.Domain.Entities;

public class DepositTests
{
    private readonly Mock<ITransactionRepository> _transactionRepoMock;
    private readonly Mock<ICuentaBancariaRepository> _cuentaRepoMock;
    private readonly Mock<ITipoTransaccionRepository> _tipoTransaccionRepoMock;

    private readonly DepositoEfectivoCommandHandler _handler;

    public DepositTests()
    {
        _transactionRepoMock = new Mock<ITransactionRepository>();
        _cuentaRepoMock = new Mock<ICuentaBancariaRepository>();
        _tipoTransaccionRepoMock = new Mock<ITipoTransaccionRepository>();

        _handler = new DepositoEfectivoCommandHandler(
            _transactionRepoMock.Object,
            _cuentaRepoMock.Object,
            _tipoTransaccionRepoMock.Object
        );
    }

    [Fact]
    public async Task DepositoEfectivo_DeberiaActualizarSaldoYRegistrarTransaccion()
    {
        // Arrange
        var cuenta = new CuentaBancaria
        {
            Id = 10,
            Numero = "123456789",
            SaldoActual = 1000m
        };

        var command = new DepositoEfectivoCommand
        {
            NumeroCuentaDestino = "123456789",
            Monto = 500m
        };

        _cuentaRepoMock.Setup(r => r.GetByNumber("123456789"))
                       .ReturnsAsync(cuenta);

        _tipoTransaccionRepoMock.Setup(r => r.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TipoTransaccion>
            {
                new() { Id = 1, Descripcion = "DEPOSITO EN EFECTIVO" }
            });

        BancaLafise.Domain.Entities.Transaccion? transaccionCreada = null;

        _transactionRepoMock
            .Setup(t => t.Create(It.IsAny<BancaLafise.Domain.Entities.Transaccion>(), It.IsAny<CancellationToken>()))
            .Callback<BancaLafise.Domain.Entities.Transaccion, CancellationToken>((t, _) => transaccionCreada = t)
            .Returns(Task.CompletedTask);

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(resultado);
        Assert.Contains("DEPOSITO EN EFECTIVO REALIZADO CON EXITO", resultado);
        Assert.Equal(1500m, cuenta.SaldoActual); // Verifica que el saldo se actualizó correctamente
        Assert.NotNull(transaccionCreada);
        Assert.Equal(500m, transaccionCreada.Monto);
        Assert.Equal(1500m, transaccionCreada.SaldoDestino);
        Assert.Equal(1, transaccionCreada.TipoTransaccion);
        Assert.Equal(10, transaccionCreada.CuentaDestino);
    }
}
