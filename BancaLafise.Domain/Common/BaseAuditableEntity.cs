namespace BancaLafise.Domain.Common
{
    public abstract class BaseAuditableEntity
    {
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;
    }
}
