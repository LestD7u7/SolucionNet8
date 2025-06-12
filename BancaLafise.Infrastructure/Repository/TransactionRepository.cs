using BancaLafise.Application.Interfaces;
using BancaLafise.Domain.Entities;
using BancaLafise.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BancaLafise.Infrastructure.Repository
{
    public class TransactionRepository : BaseRepository<Transaccion>, ITransactionRepository
    {
        public TransactionRepository(LafiseDbContext context) : base(context)
        {
        }

        public async Task<bool> validMontoDebito(CuentaBancaria cuenta, decimal montoDebito)
        {
            return cuenta.SaldoActual >= montoDebito; 
        }

        public async Task<List<Transaccion>> GetAllbyCuenta(int CuentaId, CancellationToken cancellationToken)
        {
            return await _context.Transacciones.Where(x => x.CuentaOrigen == CuentaId || x.CuentaDestino == CuentaId).ToListAsync(cancellationToken);
        }
    }
}
