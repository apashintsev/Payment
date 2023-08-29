using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Domain.Config;

namespace Payment.Infrastructure;

public static class ConfigMappingExtension
{
    public static void AddConfigMapping(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        services.Configure<PasswordSettings>(configuration.GetSection(nameof(PasswordSettings)));
        services.Configure<RpcSettings>(configuration.GetSection(nameof(RpcSettings)));

    }
}
