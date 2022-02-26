namespace Baskets.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly IHttpContextAccessor _context;

    public IdentityService(IHttpContextAccessor context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public string GetUserIdentity()
    {
        if (IsAuthenticated())
            return _context.HttpContext.User?.FindFirst("sub")?.Value;

        return null;
    }

    public string GetUserName()
    {
        if (IsAuthenticated())
            return _context.HttpContext.User.Identity.Name;


        return null;
    }

    public bool HasAdminRole()
    {
        return _context.HttpContext.User?.IsInRole("admin") ?? false;
    }

    public bool HasUserRole()
    {
        return _context.HttpContext.User?.IsInRole("user") ?? false;
    }

    public bool IsAuthenticated()
    {
        return _context.HttpContext.User?.Identity.IsAuthenticated ?? false;
    }
}
