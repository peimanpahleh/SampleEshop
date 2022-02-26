namespace BlazorApp;

public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public CustomAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation) : base(provider, navigation)
    {
        ConfigureHandler(
        authorizedUrls: new[] { "http://127.0.0.1:5020" },
        scopes: new[] { "product", "order", "basket", "roles" });
    }
}
