using Payment.Domain.Entities;

namespace Payment.Infrastructure.Services;

public interface IAuthService
{
    Task<bool> CheckPasswordSignInAsync(Merchant user, string password);
    Task<bool> CreateUser(Merchant user, string password);
    Task<Merchant> FindByEmailAsync(string email);
}