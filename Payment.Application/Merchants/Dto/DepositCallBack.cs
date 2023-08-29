using Newtonsoft.Json;

namespace Payment.Application.Notifications.Dto
{
    public class DepositCallBack : CommonCallBack
    {
        [JsonProperty(PropertyName = "crypto_address")]
        public CryptoAddressDeposit CryptoAddressDeposit { get; set; }

        [JsonProperty(PropertyName = "currency_sent")]
        public CurrencySent CurrencySent { get; set; }

        [JsonProperty(PropertyName = "currency_received")]
        public CurrencyReceived CurrencyReceived { get; set; }

        [JsonProperty(PropertyName = "transactions")]
        public Transaction[] Transactions { get; set; }

        [JsonProperty(PropertyName = "fees")]
        public Fee[] Fees { get; set; }
    }

    public class CryptoAddressDeposit : CryptoAddress
    {
        [JsonProperty(PropertyName = "foreign_id")]
        public string ForeignId { get; set; }
    }

    public class CurrencySent
    {
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public decimal Amount { get; set; }
    }

    public class CurrencyReceived : CurrencySent
    {
        [JsonProperty(PropertyName = "amount_minus_fee")]
        public decimal AmountMinusFee { get; set; }
    }

    public class TransactionDeposit : Transaction
    {
        [JsonProperty(PropertyName = "riskscore")]
        public string Riskscore { get; set; }
    }

    public class Fee
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public decimal Amount { get; set; }
    }

    public class Transaction
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "transaction_type")]
        public string TransactionType { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public string Amount { get; set; }

        [JsonProperty(PropertyName = "txid")]
        public string Txid { get; set; }

        [JsonProperty(PropertyName = "confirmations")]
        public int Confirmations { get; set; }
    }

    public class CryptoAddress
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }
    }

}
