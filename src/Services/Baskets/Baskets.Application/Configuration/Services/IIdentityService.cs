namespace Baskets.Application.Configuration.Services;

public interface IIdentityService
{
    string GetUserIdentity();
    string GetUserName();
    bool IsAuthenticated();
    bool HasAdminRole();
    bool HasUserRole();

}
