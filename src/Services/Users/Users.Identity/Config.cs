using Duende.IdentityServer.Models;

namespace Users.Identity;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("roles", new[] { "role" })
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("scope1"),
            new ApiScope("scope2"),
            new ApiScope("product", "Access to Wallet API"),
            new ApiScope("basket", "Access to Basket API"),
            new ApiScope("order", "Access to Order API"),
        };

    public static IEnumerable<ApiResource> ApiResources =>
    new ApiResource[]
    {
       new ApiResource("product-api", "Product API")
       {
           Scopes = { "product" }
       },
       new ApiResource("basket-api", "Basket API")
       {
           Scopes = { "basket" }
       },
       new ApiResource("order-api", "Order API")
       {
           Scopes = { "order" }
       }
    };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                AllowedScopes = { "scope1" }
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "interactive",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:44300/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "scope2" }
            },
            new Client
                {
                    ClientId = "blazor",
                    RequireClientSecret = false,

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "http://localhost:5019/authentication/login-callback" },
                    FrontChannelLogoutUri = "http://localhost:5019/authentication/logout-callback",
                    PostLogoutRedirectUris = { "http://localhost:5019/authentication/logout-callback" },

                    AllowOfflineAccess = true,
                    AllowedScopes = {
                    "openid",
                    "profile",
                    "scope2" ,
                    "roles",
                    "product",
                    "basket",
                    "order",
                    }
                }
        };
}