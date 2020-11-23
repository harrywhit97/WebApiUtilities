using Microsoft.Extensions.Configuration;

namespace WebApiUtilities.Concrete
{
    public class AppSettings
    {
        public string JWTKey { get; set; }
        public string SystemUserName { get; set; }
        public string SystemUserPassword { get; set; }
        public string SystemUserEmail { get; set; }

        public AppSettings(IConfiguration configuration)
        {
            JWTKey = configuration[$"AppSettings:{nameof(JWTKey)}"];
            SystemUserName = "systemuser";
            SystemUserPassword = "Test1!";
            SystemUserEmail = "system@user.com";
        }
    }
}
