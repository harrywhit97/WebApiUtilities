namespace WebApiUtilities.Interfaces
{
    public interface IAppSettings
    {
        string HttpUrl { get; set; }
        string HttpsUrl { get; set; }
        string JWTKey { get; set; }
        string SystemUserName { get; set; }
        string SystemUserPassword { get; set; }
        string SystemUserEmail { get; set; }
        string IdentityClientId { get; set; }
        string IdentityClientName { get; set; }
        string IdentityClientSecret { get; set; }
        string IdentityApiName { get; set; }
        string IdentityApiDisplayName { get; set; }
    }
}
