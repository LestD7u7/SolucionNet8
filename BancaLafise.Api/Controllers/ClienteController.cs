using BancaLafise.Application.Common.Dtos;
using BancaLafise.Application.Features.Cliente.Commands;
using BancaLafise.Application.Features.Cliente.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BancaLafise.Api.Controllers
{
    [Route("Cliente")]
    [ApiController]
    public class ClienteController : BaseController
    {
        [HttpPost("registro-cliente")]
        public async Task<ActionResult<string>> Registro([FromBody] CrearClienteCommand command)
        {
            return await Sender.Send(command);
        }

        [HttpPost("generar-token")]
        public async Task<ActionResult<TokenResponseDto>> Login([FromQuery] InicioSesionCommand command)
        {
            return await Sender.Send(command);
        }

        [Authorize]
        [HttpPost("agregar-cuenta-bancaria")]
        public async Task<ActionResult<string>> AgregarCuenta([FromQuery] AgregarCuentaBancariaCommand command)
        {
            return await Sender.Send(command);
        }

        [HttpGet("consultar-saldo-cuenta")]
        public async Task<ActionResult<CuentasBancariasResponseDto>> ConsultarCuenta([FromQuery] ObtenerCuentaBancariaByNumeroQuery command)
        {
            return await Sender.Send(command);
        }

        [HttpGet("consultar-cuentas-disponibles")]
        public async Task<ActionResult<List<CuentaBancariaDto>>> ConsultarCuentas()
        {
            return await Sender.Send(new ObtenerCuentasBancariasQuery());
        }
    }
}
