using MediatR;
using Payment.Application.Auth.Commands;
using Payment.Application.Auth.Dtos;
using Payment.Infrastructure;

namespace Payment.Application.Behaviors.Transaction;

public class SignUpCommandTransactionBehavior :
    TransactionBehavior<SignUpCommand, AuthResultVm>
{
    public SignUpCommandTransactionBehavior(ApplicationDbContext context)
        : base(context) { }
}