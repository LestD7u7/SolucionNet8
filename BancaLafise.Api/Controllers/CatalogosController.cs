using BancaLafise.Application.Common.Dtos;
using BancaLafise.Application.Features.AdminCatalogos;
using BancaLafise.Application.Features.Cliente.Commands;
using Microsoft.AspNetCore.Mvc;

namespace BancaLafise.Api.Controllers
{
    [Route("Catalogo")]
    [ApiController]
    public class CatalogosController : BaseController
    {
        [HttpPost("registro-catalogos")]
        public async Task<ActionResult<string>> Registro()
        {
            return await Sender.Send(new InsertarCatalogosBaseCommand());
        }

        [HttpGet("catalogos")]
        public async Task<ActionResult<List<CatalogosResponseDto>>> Obtener()
        {
            return await Sender.Send(new ConsultarCatalogosBaseQuery());
        }
    }
}
