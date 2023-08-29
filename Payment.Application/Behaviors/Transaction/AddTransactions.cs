using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Payment.Application.Auth.Commands;
using Payment.Application.Auth.Dtos;

namespace Payment.Application.Behaviors.Transaction;

public static class AddTransactions
{
    public static void AddTransactionsBehavior(this IServiceCollection services)
    {
        services.AddTransient<IPipelineBehavior<SignUpCommand, AuthResultVm>, SignUpCommandTransactionBehavior>();

    }
}
