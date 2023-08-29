namespace Payment.Domain.Entities;

public class Referral : BaseEntity
{
    public Guid UserId { get; private set; } //связь с пользователем, который привлек реферала
    public Guid ReferralUserId { get; private set; } //связь с рефералом
    public Merchant User { get; }
    public Merchant ReferralUser { get; }

    private Referral()
    {

    }

    /// <summary>
    /// Создать реферальную связь
    /// </summary>
    /// <param name="user">Кто привлёк реферала</param>
    /// <param name="referralUser">Приведённый пользователь</param>
    public Referral(Merchant user, Merchant referralUser)
    {
        UserId = user.Id;
        User = user;
        ReferralUserId = referralUser.Id;
        ReferralUser = referralUser;
    }
}
