using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

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
