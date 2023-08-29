using MediatR;
using Microsoft.EntityFrameworkCore;
using Payment.Domain.Entities;
using Payment.Infrastructure;
using Payment.Infrastructure.Services;

namespace Payment.Application.Merchants;

public record GetMerchantByPublicKeyQuery(string PublicKey) : IRequest<Merchant>;

public class GetMerchantByPublicKeyQueryHandler : IRequestHandler<GetMerchantByPublicKeyQuery, Merchant>
{
    private readonly ApplicationDbContext _context;
    private readonly IEncryptionService _encryptionService; // Service for handling encryption

    public GetMerchantByPublicKeyQueryHandler(ApplicationDbContext context, IEncryptionService encryptionService)
    {
        _context = context;
        _encryptionService = encryptionService;
    }

    public async Task<Merchant> Handle(GetMerchantByPublicKeyQuery request, CancellationToken cancellationToken)
    {
        var merchant = await _context.Merchants.FirstOrDefaultAsync(m => m.PublicKey == request.PublicKey);

        if (merchant != null)
        {
            // Decrypt the secret key
            merchant.SecretKey = _encryptionService.Decrypt(merchant.SecretKey);
        }

        return merchant;
    }
}
