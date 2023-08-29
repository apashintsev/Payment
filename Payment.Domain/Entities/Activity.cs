using Payment.Domain.Entities;

namespace Payment.Domain.Entities;

public class Activity:BaseEntity
{
    public Guid UserId { get; private set; }

    public string ActionType { get; private set; }

    public string Currency { get; private set; }
}
