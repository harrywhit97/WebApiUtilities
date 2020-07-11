using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApiUtilities.Abstract
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiController : ControllerBase
    {
        private IMediator mediator;
        protected IMediator Mediator => mediator ??= HttpContext.RequestServices.GetService(typeof(IMediator)) as IMediator;
    }
}
