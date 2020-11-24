using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiUtilities.Interfaces
{
    public interface IAppSettings
    {
        string JWTKey { get; set; }
        string SystemUserName { get; set; }
        string SystemUserPassword { get; set; }
        string SystemUserEmail { get; set; }
    }
}
