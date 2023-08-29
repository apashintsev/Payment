namespace Payment.Domain.Entities;

public class UserAccount : BaseEntity
{
    public Guid UserId { get; private set; }
    public decimal InvestedBalance { get; private set; }
    public decimal AccumulatedProfit { get; private set; }
    public Merchant User { get; private set; }
}
