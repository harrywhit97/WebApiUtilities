using Microsoft.Extensions.Configuration;

namespace WebApiUtilities.Concrete
{
    public class AppSettings
    {
        public string JWTKey { get; set; }

        public AppSettings(IConfiguration configuration)
        {
            JWTKey = configuration[$"AppSettings:{nameof(JWTKey)}"];
        }
    }
}
