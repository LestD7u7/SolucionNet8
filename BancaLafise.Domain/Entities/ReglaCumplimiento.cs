using BancaLafise.Domain.Common;

namespace BancaLafise.Domain.Entities
{
    public class ReglaCumplimiento : BaseEntity
    {
        public string Descripcion { get; set; } = null!;
        public string Valor { get; set; } = null!;
        public int TipoCuenta { get; set; }

    }
}
