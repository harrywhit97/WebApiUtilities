using IdentityServer4.Models;
using System.Collections.Generic;

namespace WebApiUtilities.Identity
{
    public interface IIdentityConfig
    {
        IEnumerable<string> AllowedGrantTypes { get; set; }
        IEnumerable<string> AllowedScopes { get; set; }
        bool AllowOfflineAccess { get; set; }
        IEnumerable<ApiResource> ApiResources { get; set; }
        IEnumerable<ApiScope> ApiScopes { get; set; }
        string ClientId { get; set; }
        string ClientName { get; set; }
        IEnumerable<Client> Clients { get; set; }
        IEnumerable<Secret> ClientSecrets { get; set; }
        IEnumerable<IdentityResource> IdentityResources { get; set; }
        IEnumerable<string> PostLogoutRedirectUris { get; set; }
        IEnumerable<string> RedirectUris { get; set; }
        bool RequireConsent { get; set; }
    }
}