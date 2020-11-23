using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiUtilities.Abstract
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnsecuredController : ControllerBase
    {
    }
}
