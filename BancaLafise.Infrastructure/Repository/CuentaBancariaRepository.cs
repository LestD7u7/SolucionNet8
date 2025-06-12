using BancaLafise.Application.Interfaces;
using BancaLafise.Domain.Entities;
using BancaLafise.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BancaLafise.Infrastructure.Repository
{
    public class CuentaBancariaRepository : BaseRepository<CuentaBancaria>, ICuentaBancariaRepository
    {
        public CuentaBancariaRepository(LafiseDbContext context) : base (context)
        { 
        }

        public async Task<CuentaBancaria> GetByNumber(string Numero)
        {
            return await _context.CuentasBancarias.FirstOrDefaultAsync(x => x.Numero == Numero);
        }

        public async Task<bool> Valid(string Numero, int ClienteId)
        {
            return await _context.CuentasBancarias.AnyAsync(u => u.Numero == Numero && u.ClienteId == ClienteId);
        }
    }
}
