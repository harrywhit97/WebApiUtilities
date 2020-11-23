using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiUtilities.Identity
{
    public class IdentityConfig
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public IEnumerable<string> AllowedGrantTypes { get; set; }
        public bool RequireConsent { get; set; }
        public bool AllowOfflineAccess { get; set; }
        public IEnumerable<Secret> ClientSecrets { get; set; }
        public IEnumerable<string> RedirectUris { get; set; }
        public IEnumerable<string> PostLogoutRedirectUris { get; set; }
        public IEnumerable<string> AllowedScopes { get; set; }
        public IEnumerable<ApiScope> ApiScopes { get; set; }
        public IEnumerable<Client> Clients{ get; set; }
        public static IdentityConfig Default => GetDefault();
        
        private static IdentityConfig GetDefault()
        {
            return new IdentityConfig()
            {
                ClientId = "client",
                ClientName = "Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                RequireConsent = false,
                ClientSecrets = new List<Secret> { new Secret("secret") },
                RedirectUris = new List<string>() { "http://localhost:5002/signin-oidc" },
                PostLogoutRedirectUris = new List<string>() { "http://localhost:5002/signout-callback-oidc" },
                AllowedScopes = new List<string>()
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "api1",
                    "My API"
                },
                ApiScopes = new List<ApiScope>() { new ApiScope("api1", "My API") },
                AllowOfflineAccess = true,
                Clients = new List<Client>
                {
                    new Client
                    {
                        ClientId = "client",

                        // no interactive user, use the clientid/secret for authentication
                        AllowedGrantTypes = GrantTypes.ClientCredentials,

                        // secret for authentication
                        ClientSecrets =
                        {
                            new Secret("secret")
                        },

                        // scopes that client has access to
                        AllowedScopes = { "api1" }
                    }
                }
        };
        }
    }
}
