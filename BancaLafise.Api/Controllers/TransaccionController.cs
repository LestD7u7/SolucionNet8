using BancaLafise.Application.Common.Dtos;
using BancaLafise.Application.Features.Cliente.Commands;
using BancaLafise.Application.Features.Transaccion.Commands;
using BancaLafise.Application.Features.Transaccion.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BancaLafise.Api.Controllers
{
    [Route("Transaccion")]
    [ApiController]
    public class TransaccionController : BaseController
    {
        [HttpPost("deposito-efectivo")]
        public async Task<ActionResult<string>> DepositoEfectivo([FromQuery] DepositoEfectivoCommand command)
        {
            return await Sender.Send(command);
        }

        [Authorize]
        [HttpPost("deposito-a-tercero")]
        public async Task<ActionResult<string>> DepositoTercero([FromQuery] DepositoEntreCuentasTercerosCommand command)
        {
            return await Sender.Send(command);
        }

        [Authorize]
        [HttpPost("retiro-efectivo")]
        public async Task<ActionResult<string>> RetiroEfectivo([FromQuery] RetiroEfectivoCommand command)
        {
            return await Sender.Send(command);
        }

        [HttpPost("aplicar-interes-compuesto")]
        public async Task<ActionResult<string>> AplicaInteres([FromQuery] AplicarInteresCompuestoCommand command)
        {
            return await Sender.Send(command);
        }

        [HttpGet("historial-transacciones-cuenta")]
        public async Task<ActionResult<HistorialTransaccionesResponseDto>> HistorialCuenta([FromQuery] HistorialTransaccionesCuentaQuery  query)
        {
            return await Sender.Send(query);
        }
    }
}
