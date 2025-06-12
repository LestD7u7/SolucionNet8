using BancaLafise.Domain.Common;

namespace BancaLafise.Domain.Entities
{
    public class TipoTransaccion : BaseEntity
    {
        public string Descripcion { get; set; } = null!;
        public int TipoMovimiento { get; set; }
    }
}
