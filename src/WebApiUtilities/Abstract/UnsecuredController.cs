using Microsoft.AspNetCore.Mvc;

namespace WebApiUtilities.Abstract
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class UnsecuredController : ControllerBase
    {
    }
}
