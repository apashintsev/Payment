using Payment.Domain.Entities;

namespace Payment.Infrastructure.Services;

public interface ITokenService
{
    Task<string> CreateToken(Merchant user, string role = "");
}