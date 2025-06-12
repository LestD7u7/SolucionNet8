using BancaLafise.Domain.Common;

namespace BancaLafise.Domain.Entities
{
    public class TipoCuenta : BaseEntity
    {
        public string Descripcion { get; set; } = null!;
    }
}
