using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Identity
{
    public class IdentityConfig : IIdentityConfig
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
        public IEnumerable<Client> Clients { get; set; }

        public IEnumerable<IdentityResource> IdentityResources { get; set; }

        public static IdentityConfig GetDefault(IAppSettings appSettings)
        {
            return new IdentityConfig()
            {
                ClientId = appSettings.IdentityClientId,
                ClientName = appSettings.IdentityClientName,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireConsent = false,
                ClientSecrets = new List<Secret> { new Secret(appSettings.IdentityClientSecret.Sha256()) },
                RedirectUris = new List<string>() { $"{appSettings.HttpUrl}/signin-oidc" },
                PostLogoutRedirectUris = new List<string>() { $"{appSettings.HttpUrl}/signout-callback-oidc" },
                AllowedScopes = new List<string>()
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Address,
                    appSettings.IdentityApiName,
                    appSettings.IdentityApiDisplayName
                },
                ApiScopes = new List<ApiScope>() { new ApiScope(appSettings.IdentityApiName, appSettings.IdentityApiDisplayName) },
                ApiResources = new List<ApiResource>() { new ApiResource(appSettings.IdentityApiName, appSettings.IdentityApiDisplayName) },
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
                        ClientId = appSettings.IdentityClientId,
                        RedirectUris = { appSettings.HttpsUrl },
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
                            new Secret(appSettings.IdentityClientSecret.Sha256())
                        },

                        // scopes that client has access to
                        AllowedScopes = { appSettings.IdentityApiName },
                    }
                }
            };
        }
    }
}
