using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Payment.Domain.Config;
using Payment.Domain.Enums;
using Payment.Infrastructure;
using Transaction = Payment.Domain.Entities.Transaction;

namespace Payment.Application.Addresses;


public record CheckAddressCommand(Currency Currency, string Address) : IRequest;

public class CheckAddressCommandHandler : IRequestHandler<CheckAddressCommand>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CheckAddressCommandHandler> _logger;
    private readonly RpcSettings _rpcSettings;

    public CheckAddressCommandHandler(IOptions<RpcSettings> settings, ILogger<CheckAddressCommandHandler> logger,
                                      ApplicationDbContext context
                                      )
    {
        _logger = logger;
        _context = context;
        _rpcSettings = settings.Value;
    }

    public async Task Handle(CheckAddressCommand request, CancellationToken cancellationToken)
    {
        var wallet = _context.ClientWallets.FirstOrDefault(x => x.Currency == request.Currency.ToString() && x.Address == request.Address);
        if (wallet == null)
        {
            _logger.LogError($"No wallet found for currency {request.Currency} and address {request.Address}");
            return;
        }

        var web3 = new Web3("https://ropsten.infura.io/myInfura");
        var balanceReq = await web3.Eth.GetBalance.SendRequestAsync(wallet.Address);
        var balance = 0M;
        if (balanceReq is null)
        {
            throw new Exception("smth wrong");
        }

        balance = Web3.Convert.FromWei(balanceReq.Value);
        if (balance > 0) // Проверяем, что на балансе есть средства
        {
            string toAddress = "0x1234..."; // Замените на ваш главный адрес
            var network = _rpcSettings.NetworkSettings.FirstOrDefault(x => x.Currency == request.Currency.ToString()) ?? throw new Exception("net not found");
            var account = new Account(wallet.PrivateKey, network.ChainId);
            web3 = new Web3(account, network.RpcUrl);

            var transaction = await web3.Eth.GetEtherTransferService()
            .TransferEtherAndWaitForReceiptAsync(toAddress, balance);

            // Создаем запись об операции перевода средств
            var transfer = new Transaction(wallet.Address, request.Currency, balance, transaction.TransactionHash);

            _context.Transactions.Add(transfer);
            await _context.SaveChangesAsync();

            // Отслеживание подтверждения транзакции
            TransactionReceipt receipt = null;
            while (receipt == null)
            {
                receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transaction.TransactionHash);
                if (receipt == null)
                {
                    await Task.Delay(10000); // Подождите 10 секунд перед повторной проверкой
                }
            }
            if (receipt.Status.Value == 1) { transfer.SetCompleted(); } else { transfer.SetFailed(); }
            _context.Transactions.Update(transfer);
            await _context.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation($"No funds to transfer for wallet with address {wallet.Address}");
        }
    }
}
