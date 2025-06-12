using BancaLafise.Domain.Common;

namespace BancaLafise.Domain.Entities
{
    public class Sexo : BaseEntity
    {
        public string Descripcion { get; set; } = null!;
    }
}
