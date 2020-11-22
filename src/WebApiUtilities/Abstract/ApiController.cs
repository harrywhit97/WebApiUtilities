using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApiUtilities.Abstract
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiController : ControllerBase
    {
        IMediator mediator;
        protected IMediator Mediator => mediator ??= HttpContext.RequestServices.GetService(typeof(IMediator)) as IMediator;
    }
}
