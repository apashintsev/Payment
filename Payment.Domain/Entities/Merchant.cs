using System.Numerics;
using System.Text.RegularExpressions;

namespace Payment.Domain.Entities;

public class Merchant : BaseEntity
{
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Residency { get; private set; }
    public string Citizenship { get; private set; }
    public string Occupation { get; private set; }
    public string TemporaryResidencePermit { get; private set; }
    public bool KycStatus { get; private set; }
    public string ReferralLink { get; private set; }

    public ICollection<Referral> ReferredUsers { get; }
    public ICollection<ReferralProfit> ReferralProfits { get; }

    public ICollection<ClientWallet> ClientWallets { get; }

    public UserAccount UserAccount { get; private set; }
    public string PublicKey { get; private set; }
    public string SecretKey { get; set; }

    public string CallbackUrl { get; private set; }

    private Merchant()
    {
    }

    public Merchant(string email, string phone)
    {
        phone = CleanPhoneNumber(phone);
        if (!IsValidEmail(email)) throw new Exception("Email is not valid!");
        if (!IsValidPhone(phone)) throw new Exception("Phone is not valid!");
        Email = email;
        Phone = phone;
        ReferralLink = CreateRefUrl();
    }

    private string CreateRefUrl()
    {
        // Преобразование массива байтов в BigInteger
        BigInteger bigInt = new(Guid.NewGuid().ToByteArray());
        // Кодирование BigInteger в base62
        return EncodeToBase62(bigInt).ToString();
    }

    private string EncodeToBase62(BigInteger value)
    {
        const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string result = "";
        value = BigInteger.Abs(value);
        while (value > 0)
        {
            int remainder = (int)(value % 62);
            value /= 62;
            result = chars[remainder] + result;
        }
        return result;
    }

    public static bool IsValidEmail(string email)
    {
        // Use a regular expression pattern to match against the email string
        // This pattern is based on the RFC 5322 specification for email addresses
        // Note that this pattern is not perfect and may not catch all invalid email addresses
        string pattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";
        Regex regex = new(pattern);
        return regex.IsMatch(email);
    }

    public static bool IsValidPhone(string phone)
    {
        //is for matching international phone numbers that start with a plus sign (+), followed by between 6 to 14 digits
        string pattern = @"^\+(?:[0-9]●?){6,14}[0-9]$";
        Regex regex = new(pattern);
        return regex.IsMatch(phone);
    }

    /// <summary>
    /// Очищает телефонный номер от пробелов и скобок, оставляя только плюс впереди и цифры номера
    /// </summary>
    /// <param name="um"></param>
    /// <param name="phone"></param>
    /// <returns></returns>
    public static string CleanPhoneNumber(string phone)
    {
        var digitsOnly = new Regex(@"[^\d]");
        return $"+{digitsOnly.Replace(phone, "")}";
    }

    public void SetCallbackUrl(string url)
    {
        CallbackUrl = url;
    }
}
