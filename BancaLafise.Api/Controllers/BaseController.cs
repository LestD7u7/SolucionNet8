using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BancaLafise.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        private ISender _sender;

        protected ISender Sender => _sender ??= HttpContext.RequestServices.GetService<ISender>();
    }
}
