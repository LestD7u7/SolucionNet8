using BancaLafise.Domain.Entities;

namespace BancaLafise.Application.Common.Dtos
{
    public class HistorialTransaccionesResponseDto
    {
        public CuentaBancariaDto Cuenta { get; set; }
        public List<TransaccionDto> Transacciones { get; set; }
    }

    public class TransaccionDto
    {
        public string Referencia { get; set; }
        public string TipoMovimiento { get; set; }
        public string TipoTransaccion { get; set; }
        public string Monto { get; set; }
        public string Saldo { get; set; }
        public string Fecha { get; set; }
    }
}
