using Moq;
using BancaLafise.Application.Features.Transaccion.Queries;
using BancaLafise.Application.Interfaces;
using BancaLafise.Domain.Entities;

public class HistorialTransaccionesTests
{
    private readonly Mock<ITransactionRepository> _transactionRepoMock = new();
    private readonly Mock<ICuentaBancariaRepository> _cuentaRepoMock = new();
    private readonly Mock<ITipoTransaccionRepository> _tipoTransaccionRepoMock = new();
    private readonly Mock<ITipoCuentaRepository> _tipoCuentaRepoMock = new();
    private readonly Mock<ITipoMovimientoRepository> _tipoMovimientoRepoMock = new();

    private readonly HistorialTransaccionesCuentaQueryHandler _handler;

    public HistorialTransaccionesTests()
    {
        _handler = new HistorialTransaccionesCuentaQueryHandler(
            _transactionRepoMock.Object,
            _cuentaRepoMock.Object,
            _tipoTransaccionRepoMock.Object,
            _tipoCuentaRepoMock.Object,
            _tipoMovimientoRepoMock.Object
        );
    }

    [Fact]
    public async Task HistorialCuenta_DeberiaRetornarTransacciones()
    {
        // Arrange
        var cuenta = new CuentaBancaria
        {
            Id = 1,
            Numero = "987654",
            SaldoActual = 800m,
            TipoCuenta = 2
        };

        var transacciones = new List<Transaccion>
        {
            new()
            {
                NumeroReferencia = "REF1",
                Monto = 200,
                CuentaDestino = 1,
                SaldoDestino = 800,
                TipoTransaccion = 1,
                FechaRegistro = DateTime.Now
            },
            new()
            {
                NumeroReferencia = "REF2",
                Monto = 100,
                CuentaOrigen = 1,
                SaldoOrigen = 700,
                TipoTransaccion = 2,
                FechaRegistro = DateTime.Now
            }
        };

        var tipoTransacciones = new List<TipoTransaccion>
        {
            new() { Id = 1, Descripcion = "DEPOSITO" },
            new() { Id = 2, Descripcion = "RETIRO" }
        };

        var tipoCuentas = new List<TipoCuenta>
        {
            new() { Id = 2, Descripcion = "CUENTA AHORRO" }
        };

        _cuentaRepoMock.Setup(r => r.GetByNumber("987654"))
            .ReturnsAsync(cuenta);

        _transactionRepoMock.Setup(r => r.GetAllbyCuenta(cuenta.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(transacciones);

        _tipoTransaccionRepoMock.Setup(r => r.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tipoTransacciones);

        _tipoCuentaRepoMock.Setup(r => r.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tipoCuentas);

        _tipoMovimientoRepoMock.Setup(r => r.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TipoMovimiento>());

        var query = new HistorialTransaccionesCuentaQuery { NumeroCuenta = "987654" };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal("987654", result.Cuenta.Numero);
        Assert.Equal(2, result.Transacciones.Count);
        Assert.Contains(result.Transacciones, x => x.TipoMovimiento == "CREDITO");
        Assert.Contains(result.Transacciones, x => x.TipoMovimiento == "DEBITO");
        Assert.Equal("CUENTA AHORRO", result.Cuenta.TipoCuenta);
    }

    [Fact]
    public async Task HistorialCuenta_DeberiaLanzarError_SiCuentaNoExiste()
    {
        // Arrange
        var query = new HistorialTransaccionesCuentaQuery { NumeroCuenta = "000000" };

        _cuentaRepoMock.Setup(r => r.GetByNumber("000000"))
            .ReturnsAsync((CuentaBancaria?)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(query, CancellationToken.None));
        Assert.Equal("Cuenta origen no encontrada", ex.Message);
    }
}
