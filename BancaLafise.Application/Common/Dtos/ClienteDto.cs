namespace BancaLafise.Application.Common.Dtos
{
    public class ClienteDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int SexoId { get; set; }
        public decimal MontoIngreso { get; set; }
    }
}
