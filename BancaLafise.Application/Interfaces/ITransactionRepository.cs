using BancaLafise.Domain.Entities;

namespace BancaLafise.Application.Interfaces
{
    public interface ITransactionRepository : IBaseRepository<Transaccion>
    {
        Task<List<Transaccion>> GetAllbyCuenta(int CuentaId ,CancellationToken cancellationToken);
        Task<bool> validMontoDebito(CuentaBancaria cuenta ,decimal montoDebito);
    }
}
