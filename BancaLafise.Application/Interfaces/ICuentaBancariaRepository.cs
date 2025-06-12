using BancaLafise.Domain.Entities;

namespace BancaLafise.Application.Interfaces
{
    public interface ICuentaBancariaRepository : IBaseRepository<CuentaBancaria>
    {
        Task<CuentaBancaria> GetByNumber(string Numero);
        Task<bool> Valid(string Numero, int ClienteId);
    }
}
