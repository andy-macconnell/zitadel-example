namespace Authentication.Web.Options;

public sealed class ZitadelOptions
{
    public static string Section = "Zitadel";

    public required string BaseUri { get; init; }
    public required string AccessToken { get; init; }
    public string ClientId { get; init; }
    public string ClientSecret { get; init; }
}
