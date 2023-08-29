using MediatR;
using Payment.Domain.Enums;

namespace Payment.Domain.Events;

public class FundsReceivedNotification : INotification
{
    public Guid MerchantId { get; set; }
    public string TransactionId { get; set; }
    public string WalletAddress { get; set; }
    public Currency Currency { get; set; }
    public decimal Amount { get; set; }
    public string ForeignId { get; set; }

    public FundsReceivedNotification(Guid merchantId, string foreignId, string transactionId, string walletAddress, Currency currency, decimal amount)
    {
        TransactionId = transactionId;
        WalletAddress = walletAddress;
        Currency = currency;
        Amount = amount;
        MerchantId = merchantId;
        ForeignId = foreignId;
    }
}
