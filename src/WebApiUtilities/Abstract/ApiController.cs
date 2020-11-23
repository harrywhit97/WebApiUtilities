using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApiUtilities.Abstract
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public abstract class ApiController : UnsecuredController
    {
        IMediator mediator;
        protected IMediator Mediator => mediator ??= HttpContext.RequestServices.GetService(typeof(IMediator)) as IMediator;
    }
}
