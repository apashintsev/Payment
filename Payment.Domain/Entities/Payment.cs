namespace Payment.Domain.Entities;

public class Payment:BaseEntity
{
    public string Address { get; set; }
    public decimal Amount { get; set; }
    public string TransactionId { get; set; }
    public PaymentStatus Status { get; set; }
}

public enum PaymentStatus
{
    Pending,
    Completed
}
