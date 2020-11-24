using Microsoft.Extensions.Configuration;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Concrete
{
    public class AppSettings : IAppSettings
    {
        public string JWTKey { get; set; }
        public string SystemUserName { get; set; }
        public string SystemUserPassword { get; set; }
        public string SystemUserEmail { get; set; }

        public AppSettings(IConfiguration configuration)
        {
            JWTKey = configuration[$"AppSettings:{nameof(JWTKey)}"];
            SystemUserName = configuration[$"AppSettings:{nameof(SystemUserName)}"];
            SystemUserPassword = configuration[$"AppSettings:{nameof(SystemUserPassword)}"];
            SystemUserEmail = configuration[$"AppSettings:{nameof(SystemUserEmail)}"];
        }
    }
}
