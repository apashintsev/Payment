using MediatR;
using Microsoft.Extensions.Logging;
using Payment.Domain.Enums;
using Payment.Infrastructure.Services;
using Payment.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Payment.Application.Notifications.Dto;

namespace Payment.Application.Merchants;


public record SendDepositCallbackCommand(Guid MerchantId, string TransactionId, string WalletAddress, Currency Currency, decimal Amount, string ForeignId) : IRequest;

public class SendDepositCallbackCommandHandler : IRequestHandler<SendDepositCallbackCommand>
{
    private readonly IEncryptionService _encryptionService; // Service for handling encryption
    private readonly ApplicationDbContext _context;
    private readonly string _baseUrl;
    private readonly string _key;
    private readonly string _signature;
    private const int MaxRetryAttempts = 3;
    private int DelayBetweenRetriesInSeconds = 2;
    private readonly ILogger<SendDepositCallbackCommandHandler> _logger;

    public SendDepositCallbackCommandHandler(ILogger<SendDepositCallbackCommandHandler> logger, IEncryptionService encryptionService, ApplicationDbContext context)
    {
        _logger = logger;
        _encryptionService = encryptionService;
        _context = context;
    }

    public async Task Handle(SendDepositCallbackCommand request, CancellationToken cancellationToken)
    {
        var merchant = await _context.Merchants.FirstOrDefaultAsync(x => x.Id == request.MerchantId);
        //TODO CHECK
        var client = new RestClient(_baseUrl);
        var req = new RestRequest(merchant.CallbackUrl, Method.Post);  //("CallBackEndPoint", Method.POST, DataFormat.Json);

        var callback = new DepositCallBack()
        {
            Id = Guid.NewGuid(),
        }
        

        // Create HMAC-SHA512 signature
        string requestBody = JsonConvert.SerializeObject(callbackData);
        var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(merchant.SecretKey));//TODO MAYBE DECRYPT HERE
        var signature = BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(requestBody))).Replace("-", "").ToLower();

        req.AddHeader("X-Processing-Key", merchant.PublicKey);
        req.AddHeader("X-Processing-Signature", signature);
        req.AddHeader("Content-Type", "application/json");
        req.AddJsonBody(requestBody);

        for (int retry = 0; retry < MaxRetryAttempts; retry++)
        {
            var response = await client.ExecuteAsync(req);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                // Запрос был успешным, выходим из цикла повторов
                return;
            }
            else
            {
                // Запрос не удался. Если это была последняя попытка, выбросить исключение.
                if (retry == MaxRetryAttempts - 1)
                {
                    throw new Exception($"Notification failed after {MaxRetryAttempts} attempts with status code {response.StatusCode}");
                }

                // Подождите перед следующей попыткой
                await Task.Delay(TimeSpan.FromSeconds(DelayBetweenRetriesInSeconds));
                DelayBetweenRetriesInSeconds *= DelayBetweenRetriesInSeconds;
            }
        }
    }
}
