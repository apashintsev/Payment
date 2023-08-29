namespace Payment.Domain.Entities;

public class WithdrawalHistory : BaseEntity
{
    public Guid UserId { get; private set; }
    public DateTime WithdrawalDate { get; private set; }
    public string WithdrawalMethod { get; private set; }
    public string Address { get; private set; }

    public decimal Amount { get; private set; }

    public string Status { get; private set; }
}
