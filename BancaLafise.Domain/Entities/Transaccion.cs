using BancaLafise.Domain.Common;

namespace BancaLafise.Domain.Entities
{
    public class Transaccion : BaseEntity
    {
        public string NumeroReferencia { get; set; } = null!;
        public decimal Monto { get; set; }
        public decimal SaldoOrigen { get; set; }
        public decimal SaldoDestino { get; set; }
        public int CuentaOrigen { get; set; }
        public int CuentaDestino { get; set; }
        public int TipoTransaccion { get; set; }
    }
}
