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
        public IEnumerable<ApiResource> ApiResources { get; set; }
        public IEnumerable<Client> Clients{ get; set; }
        public static IdentityConfig Default => GetDefault();

        public IEnumerable<IdentityResource> IdentityResources { get; set; }

        private static IdentityConfig GetDefault()
        {
            return new IdentityConfig()
            {
                ClientId = "client",
                ClientName = "Client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireConsent = false,
                ClientSecrets = new List<Secret> { new Secret("secret".Sha256()) },
                RedirectUris = new List<string>() { "http://localhost:5002/signin-oidc" },
                PostLogoutRedirectUris = new List<string>() { "http://localhost:5002/signout-callback-oidc" },
                AllowedScopes = new List<string>()
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Address,
                    "api1",
                    "My API"
                },
                ApiScopes = new List<ApiScope>() { new ApiScope("api1", "My API") },
                ApiResources = new List<ApiResource>() { new ApiResource("api1", "My API") },
                IdentityResources = new List<IdentityResource> 
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Email(),
                    new IdentityResources.Profile(),
                },
                AllowOfflineAccess = true,
                Clients = new List<Client>
                {
                    new Client
                    {
                        ClientId = "client",
                        RedirectUris = { "https://localhost:5003" },
                        // no interactive user, use the clientid/secret for authentication
                        AllowedGrantTypes = 
                        {
                            GrantType.ResourceOwnerPassword,
                            GrantType.ClientCredentials,
                            GrantType.AuthorizationCode,
                        },

                        // secret for authentication
                        ClientSecrets =
                        {
                            new Secret("secret".Sha256())
                        },

                        // scopes that client has access to
                        AllowedScopes = { "api1" },
                    }
                }
            };
        }
    }
}
