namespace BancaLafise.Application.Common.Dtos
{
    public class CuentasBancariasResponseDto
    {
        public ClienteDto cliente { get; set; }
        public List<CuentaBancariaDto> cuentas { get; set; }
    }
}
