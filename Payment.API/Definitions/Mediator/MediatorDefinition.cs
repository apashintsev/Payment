using MediatR;
using Payment.API.Definitions.Common;
using Payment.Application.Auth.Commands;
using Payment.Application.Behaviors;
using Payment.Application.Behaviors.Transaction;

namespace Payment.API.Definitions.Mediator;

/// <summary>
/// Register Mediator as application definition
/// </summary>
public class MediatorDefinition : AppDefinition
{
    /// <summary>
    /// Configure services for current application
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builder"></param>
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddTransactionsBehavior();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<LoginCommand>());
    }
}