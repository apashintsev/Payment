namespace Payment.Domain.Entities;

public class ReferralProfit : BaseEntity
{
    public Guid MerchantId { get; private set; } //связь с основной моделью пользователя
    public Guid ReferralId { get; private set; } //связь с моделью реферала
    public decimal Profit { get; private set; } //прибыль от реферала
    public decimal YourIncome { get; private set; } //ваш доход

    public Merchant Merchant { get; }
    public Referral Referral { get; }
}
