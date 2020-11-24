using Microsoft.Extensions.Configuration;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Concrete
{
    public class AppSettings : IAppSettings
    {
        public string HttpUrl { get; set; }
        public string HttpsUrl { get; set; }
        public string JWTKey { get; set; }
        public string SystemUserName { get; set; }
        public string SystemUserPassword { get; set; }
        public string SystemUserEmail { get; set; }
        public string IdentityClientId { get; set; }
        public string IdentityClientName { get; set; }
        public string IdentityClientSecret { get; set; }
        public string IdentityApiName { get; set; }
        public string IdentityApiDisplayName { get; set; }

        public AppSettings(IConfiguration configuration)
        {
            HttpUrl = configuration[$"Kestrel:EndPoints:Http:Url"];
            HttpsUrl = configuration[$"Kestrel:EndPoints:Https:Url"];
            JWTKey = configuration[$"AppSettings:{nameof(JWTKey)}"];
            SystemUserName = configuration[$"AppSettings:{nameof(SystemUserName)}"];
            SystemUserPassword = configuration[$"AppSettings:{nameof(SystemUserPassword)}"];
            SystemUserEmail = configuration[$"AppSettings:{nameof(SystemUserEmail)}"];
            IdentityClientId = configuration[$"AppSettings:{nameof(IdentityClientId)}"];
            IdentityClientName = configuration[$"AppSettings:{nameof(IdentityClientName)}"];
            IdentityClientSecret = configuration[$"AppSettings:{nameof(IdentityClientSecret)}"];
            IdentityApiName = configuration[$"AppSettings:{nameof(IdentityApiName)}"];
            IdentityApiDisplayName = configuration[$"AppSettings:{nameof(IdentityApiDisplayName)}"];
        }
    }
}
