using BancaLafise.Domain.Common;

namespace BancaLafise.Domain.Entities
{
    public class CuentaBancaria : BaseEntity
    {
        public string Numero { get; set; } = null!;
        public decimal SaldoActual { get; set; }
        public int ClienteId { get; set; }
        public int TipoCuenta { get; set; }
    }
}
