using Payment.API.Definitions.Common;
using Payment.Infrastructure.Services;

namespace Payment.API.Definitions.DependencyContainer;

/// <summary>
/// Dependency container definition
/// </summary>
public class ContainerDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEncryptionService, AesEncryptionService>();

        //services.AddTransient<
        //    IPipelineBehavior<CategoryUpdateRequest, OperationResult<CategoryEditViewModel>>,
        //    CategoryUpdateRequestTransactionBehavior>();
    }
}