using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiUtilities.Identity
{
    public class AuthenticateRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
