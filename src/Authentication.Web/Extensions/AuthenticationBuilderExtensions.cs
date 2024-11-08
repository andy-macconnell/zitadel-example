using Authentication.Web.Options;
using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

using Zitadel.Authentication;

namespace Authentication.Web.Extensions;

public static class AuthenticationBuilderExtensions
{
    public static void AddZitadel(this AuthenticationBuilder builder, IConfiguration config)
    {
        builder.AddOpenIdConnect(options => ConfigureOidc(options, config));
    }

    private static OpenIdConnectOptions ConfigureOidc(OpenIdConnectOptions options, IConfiguration config)
    {
        var zitadelOptions = config
            .GetSection(ZitadelOptions.Section)
            .Get<ZitadelOptions>();

        ArgumentNullException.ThrowIfNull(zitadelOptions);

        options.Authority = zitadelOptions.BaseUri;
        options.ClientId = zitadelOptions.ClientId;
        options.ClientSecret = zitadelOptions.ClientSecret;

        options.ResponseType = OpenIdConnectResponseType.Code;
        options.SaveTokens = true;
        options.RequireHttpsMetadata = false;
        options.GetClaimsFromUserInfoEndpoint = true;

        options.Scope.Clear();
        AddOidcScopes(options);
        AddZitadelScopes(options);

        MapStandardClaimAction(options);
        MapZitadelClaimActions(options);
        MapZitadelMetadataClaimActions(options);
        // options.EventsType = typeof(CustomOpenIdConnectEvents);

        return options;
    }

    private static void AddOidcScopes(OpenIdConnectOptions options)
    {
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
    }

    private static void MapZitadelMetadataClaimActions(OpenIdConnectOptions options)
    {
        options.ClaimActions.MapUniqueJsonKey("urn:zitadel:iam:user:metadata", "userimports");
        options.ClaimActions.MapUniqueJsonKey("urn:zitadel:iam:org:metadata", "orgimports");
        options.ClaimActions.MapUniqueJsonKey("urn:zitadel:iam:org:domain:primary", "domain");
        options.ClaimActions.MapUniqueJsonKey("urn:zitadel:iam:user:resourceowner:id", "org");
    }

    private static void MapStandardClaimAction(OpenIdConnectOptions options)
    {
        options.ClaimActions.MapUniqueJsonKey("preferred_username", "preferred_username");
    }

    private static void MapZitadelClaimActions(OpenIdConnectOptions options)
    {
        // taken directly from the zitadel extension source
        options.ClaimActions.Add(new UniqueJsonKeyClaimAction(ClaimTypes.NameIdentifier, ClaimValueTypes.String, "sub"));
        options.ClaimActions.Add(new JsonKeyClaimAction(ZitadelClaimTypes.PrimaryDomain, ClaimValueTypes.String, ZitadelClaimTypes.PrimaryDomain));
        options.ClaimActions.Add(new JsonKeyClaimAction(ClaimTypes.Name, ClaimValueTypes.String, "name"));
        options.ClaimActions.Add(new JsonKeyClaimAction(ClaimTypes.GivenName, ClaimValueTypes.String, "given_name"));
        options.ClaimActions.Add(new JsonKeyClaimAction(ClaimTypes.Surname, ClaimValueTypes.String, "family_name"));
        options.ClaimActions.Add(new JsonKeyClaimAction("nickname", ClaimValueTypes.String, "nickname"));
        options.ClaimActions.Add(new JsonKeyClaimAction("preferred_username", ClaimValueTypes.String, "preferred_username"));
        options.ClaimActions.Add(new JsonKeyClaimAction("gender", ClaimValueTypes.String, "gender"));
        options.ClaimActions.Add(new JsonKeyClaimAction(ClaimTypes.Email, ClaimValueTypes.String, "email"));
        options.ClaimActions.Add(new JsonKeyClaimAction("email_verified", ClaimValueTypes.Boolean, "email_verified"));
        options.ClaimActions.Add(new JsonKeyClaimAction(ClaimTypes.Locality, ClaimValueTypes.String, "locale"));
        options.ClaimActions.Add(new JsonKeyClaimAction("locale", ClaimValueTypes.String, "locale"));
        options.ClaimActions.Add(new DeleteClaimAction(ZitadelClaimTypes.Role));
    }

    private static void AddZitadelScopes(OpenIdConnectOptions options)
    {
        options.Scope.Add("urn:zitadel:iam:org:project:id:zitadel:aud");    // audience scope. Will return clientid and project id within zitadel
        options.Scope.Add("urn:zitadel:iam:user:metadata");                 // User metadata scope (if any exist against user. Will be returned as base64 kvps
        options.Scope.Add("urn:zitadel:iam:org:metadata");                  // Organization metadata
        options.Scope.Add("urn:zitadel:iam:org:domain:primary");            // Primary domain
        options.Scope.Add("urn:zitadel:iam:user:resourceowner:id");

    }
}