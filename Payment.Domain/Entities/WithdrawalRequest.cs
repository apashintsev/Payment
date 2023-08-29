namespace Payment.Domain.Entities;

public class WithdrawalRequest : BaseEntity
{
    public Guid UserId { get; private set; }

    public decimal WithdrawalAmount { get; private set; }

    public string Status { get; private set; }

    public DateTime CompletedAt { get; private set; }
}
