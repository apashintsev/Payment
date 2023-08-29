using Payment.Domain.Enums;

namespace Payment.Domain.Entities;

public class Transaction : BaseEntity
{
    public Currency Currency { get; private set; }

    public string Address { get; private set; }

    public decimal Amount { get; private set; }

    public string TransactionHash { get; private set; }

    public TransactionStatus Status { get; private set; }

    public DateTime CompletedAt { get; private set; }


    public Transaction(string address, Currency currency, decimal amount, string transactionHash)
    {
        Address = address;
        Currency = currency;
        Amount = amount;
        Status = TransactionStatus.Pending;
        TransactionHash = transactionHash;
    }

    public void SetCompleted()
    {
        Status = TransactionStatus.Completed;
    }

    public void SetFailed()
    {
        Status = TransactionStatus.Failed;
    }
}

public enum TransactionStatus
{
    Pending,
    Completed,
    Failed
}
