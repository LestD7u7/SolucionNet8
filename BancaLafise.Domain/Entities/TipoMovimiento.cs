using BancaLafise.Domain.Common;

namespace BancaLafise.Domain.Entities
{
    public class TipoMovimiento : BaseEntity
    {
        public string Descripcion { get; set; } = null!;
    }
}
