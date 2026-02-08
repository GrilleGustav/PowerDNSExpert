using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace IdentityProvider.Config;

public static class IdentityServerConfig
{
    public static IEnumerable<IdentityResource> IdentityResources =>
    [
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email()
    ];

    public static IEnumerable<ApiScope> ApiScopes =>
    [
        new("powerdns.api", "PowerDNS API")
    ];

    public static IEnumerable<ApiResource> ApiResources =>
    [
        new("powerdns.resource", "PowerDNS Resource API")
        {
            Scopes = { "powerdns.api" }
        }
    ];

    public static IEnumerable<Client> Clients =>
    [
        new()
        {
            ClientId = "angular-client",
            ClientName = "Angular Frontend",
            AllowedGrantTypes = GrantTypes.Code,
            RequirePkce = true,
            RequireClientSecret = false,
            RedirectUris = { "http://localhost:4200/auth/callback" },
            PostLogoutRedirectUris = { "http://localhost:4200" },
            AllowedCorsOrigins = { "http://localhost:4200" },
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.Email,
                "powerdns.api"
            },
            AllowAccessTokensViaBrowser = true
        }
    ];
}
