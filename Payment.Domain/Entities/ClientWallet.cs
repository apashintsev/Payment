namespace Payment.Domain.Entities;

public class ClientWallet :BaseEntity
{
    public string Currency { get; set; }
    public string ConvertTo { get; set; }
    public string Address { get; set; }
    public string Tag { get; set; }
    public string ForeignId { get; set; }
    public string PrivateKey { get; set; }

    public Guid MerchantId { get; private set; } //связь с основной моделью пользователя

    public Merchant Merchant { get; }

    public ClientWallet(Guid merchantId, string foreignId, string address, string currency, string convertTo, string privateKey)
    {
        ForeignId = foreignId;
        Address = address;
        Currency = currency;
        ConvertTo = convertTo;
        PrivateKey = privateKey;
        MerchantId = merchantId;
    }
}
