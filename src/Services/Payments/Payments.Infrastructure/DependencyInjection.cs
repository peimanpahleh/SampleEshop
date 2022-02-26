using Microsoft.Extensions.Configuration;

namespace Payments.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ServiceSettings settings)
    {

        return services;
    }
}
