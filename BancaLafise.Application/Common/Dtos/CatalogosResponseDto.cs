namespace BancaLafise.Application.Common.Dtos
{
    public class CatalogosResponseDto
    {
        public string NombreTabla { get; set; }

        public List<object>? Registros { get; set; }
    }
}
