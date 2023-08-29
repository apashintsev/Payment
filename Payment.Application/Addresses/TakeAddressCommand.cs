using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nethereum.Hex.HexConvertors.Extensions;
using Payment.Application.Addresses.Dto;
using Payment.Domain.Entities;
using Payment.Infrastructure;

namespace Payment.Application.Addresses;

public record TakeAddressCommand(string MerchantPublicKey, string ForeignId, string Currency, string ConvertTo) : IRequest<TakeAddressResponseVm>;

public class TakeAddressCommandHandler : IRequestHandler<TakeAddressCommand, TakeAddressResponseVm>
{
    private readonly ApplicationDbContext _context;

    public TakeAddressCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TakeAddressResponseVm> Handle(TakeAddressCommand request, CancellationToken cancellationToken)
    {
        var merchant = await _context.Merchants.FirstOrDefaultAsync(m => m.PublicKey == request.MerchantPublicKey);
        if(merchant is null)
        {
            throw new Exception("mcnt ntfnd");
        }
        var wallet = await _context.ClientWallets.FirstOrDefaultAsync(x => x.ForeignId == request.ForeignId && x.Currency == request.Currency);
        if (wallet is not null)
        {
            return new TakeAddressResponseVm()
            {
                Id = wallet.Id,
                ForeignId = wallet.ForeignId,
                Address = wallet.Address,
                ConvertTo = wallet.ConvertTo,
                Currency = wallet.Currency,
                Tag = wallet.Tag
            };
        }

        string? address;
        string? privateKey;
        if (new[] { "ETH", "BNB", "MATIC" }.Contains(request.Currency))
        {
            var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
            privateKey = ecKey.GetPrivateKeyAsBytes().ToHex();
            var account = new Nethereum.Web3.Accounts.Account(privateKey);
            address = account.Address;
        }
        else
        {
            throw new NotImplementedException();
        }

        wallet = new ClientWallet(merchant.Id, request.ForeignId, address, request.Currency, request.ConvertTo, privateKey);
        _context.ClientWallets.Add(wallet);
        await _context.SaveChangesAsync();

        return new TakeAddressResponseVm()
        {
            Id = wallet.Id,
            ForeignId = wallet.ForeignId,
            Address = wallet.Address,
            ConvertTo = wallet.ConvertTo,
            Currency = wallet.Currency,
            Tag = wallet.Tag
        };
    }
}

public class TakeAddressCommandValidator : AbstractValidator<TakeAddressCommand>
{
    public TakeAddressCommandValidator()
    {
        RuleFor(m => m.MerchantPublicKey).NotNull();
        RuleFor(m => m.ForeignId).NotEmpty().WithMessage("Email required");
        RuleFor(m => m.Currency).Custom((value, context) =>
        {
            if (value != "BTC")//TODO: validation
                context.AddFailure("Invalid currency");
        });
    }
}
