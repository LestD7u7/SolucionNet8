using BancaLafise.Domain.Common;

namespace BancaLafise.Domain.Entities
{
    public class EstadoUsuario : BaseEntity
    {
        public string Descripcion { get; set; } = null!;
    }
}
