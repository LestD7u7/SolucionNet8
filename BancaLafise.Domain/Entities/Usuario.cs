using BancaLafise.Domain.Common;

namespace BancaLafise.Domain.Entities
{
    public class Usuario : BaseEntity
    {
        public string Nombre { get; set; } = null!;
        public string Clave { get; set; } = null!;
        public int ClienteId { get; set; }
        public int EstadoId { get; set; }
    }
}
