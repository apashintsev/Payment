using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Payment.Infrastructure.Services;

public interface IEncryptionService
{
    string Encrypt(string plainText);
    string Decrypt(string cipherText);
    (string, string) GenerateRandomKeys();
    string EncryptWithSignature(string message, string signature);
}

public class AesEncryptionService : IEncryptionService
{
    private static readonly byte[] Key = Encoding.UTF8.GetBytes("your-key-goes-here----"); // Key needs to be of length 16, 24 or 32
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("your-iv-goes-here-"); // IV needs to be of length 16

    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        using (var streamWriter = new StreamWriter(cryptoStream))
        {
            streamWriter.Write(plainText);
        }

        var encrypted = memoryStream.ToArray();
        return Convert.ToBase64String(encrypted);
    }

    public string Decrypt(string cipherText)
    {
        var fullCipher = Convert.FromBase64String(cipherText);

        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using var memoryStream = new MemoryStream(fullCipher);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using (var streamReader = new StreamReader(cryptoStream))
        {
            return streamReader.ReadToEnd();
        }
    }

    public (string, string) GenerateRandomKeys()
    {
        byte[] key;
        byte[] iv;
        using (var rng = RandomNumberGenerator.Create())
        {
            key = new byte[32]; // 256 bits
            rng.GetBytes(key);

            iv = new byte[16]; // 128 bits
            rng.GetBytes(iv);
        }

        return (Convert.ToBase64String(key), Convert.ToBase64String(iv));
    }

    public string EncryptWithSignature(string message, string signature)
    {
        // Create HMAC-SHA512 signature
        var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(signature));
        return BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(message))).Replace("-", "").ToLower();
    }
}
