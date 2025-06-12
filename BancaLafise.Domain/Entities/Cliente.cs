using BancaLafise.Domain.Common;

namespace BancaLafise.Domain.Entities
{
    public class Cliente : BaseEntity
    {
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        public int SexoId { get; set; }
        public decimal MontoIngreso { get; set; }
    }
}
