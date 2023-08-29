namespace Payment.Domain.Entities;

public class PlatformSettings : BaseEntity
{
    /// <summary>
    /// Текущая прибыль % месяц, начисляется раз в день
    /// </summary>
    public decimal ProfitRate { get; private set; }

    /// <summary>
    /// Текущий % прибыли от приглашенного реферала
    /// </summary>
    public decimal ReferralRate { get; private set; }

    /// <summary>
    /// сумма депозитов в долларах после которой начисляется бонус за приведенного пользователя
    /// </summary>
    public decimal RefferalTreshold { get; private set; }

    /// <summary>
    /// фиксированная плата за реферала с депозитом больше чем referral _threshold
    /// </summary>
    public decimal RefferalPayout { get; private set; }

    /// <summary>
    /// сумма выплаты за реферала
    /// </summary>
    public decimal PayoutAmount { get; private set; }
}
