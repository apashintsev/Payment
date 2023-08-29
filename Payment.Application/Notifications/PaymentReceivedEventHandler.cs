using MediatR;
using Payment.Application.Merchants;
using Payment.Domain.Events;

namespace Payment.Application.Notifications;

public class PaymentReceivedEventHandler : INotificationHandler<FundsReceivedNotification>
{
    private readonly IMediator _mediator;
    public PaymentReceivedEventHandler( IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(FundsReceivedNotification notification, CancellationToken cancellationToken)
    {

        await _mediator.Send(
            new SendDepositCallbackCommand(
                notification.MerchantId, 
                notification.TransactionId, 
                notification.WalletAddress, 
                notification.Currency, 
                notification.Amount, 
                notification.ForeignId)
            );


    }
}
