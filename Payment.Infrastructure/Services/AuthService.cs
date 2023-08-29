using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Payment.Domain.Entities;

namespace Payment.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly ApplicationDbContext _context;
    public AuthService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<bool> CheckPasswordSignInAsync(Merchant user, string password)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        if (appUser is not null)
        {
            return await _userManager.CheckPasswordAsync(appUser, password);
        }
        return false;
    }

    public async Task<bool> CreateUser(Merchant user, string password)
    {
        try
        {
            // Adding the domain user entity
            _context.Merchants.Add(user);
            await _context.SaveChangesAsync();

            var appUser = new ApplicationUser(user.Id)
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.Phone,
                UserName = user.Email
            };

            var result = await _userManager.CreateAsync(appUser, password);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public async Task<Merchant> FindByEmailAsync(string email)
    {
        var appUser = await _userManager.FindByEmailAsync(email);
        if (appUser == null)
        {
            return null;
        }
        return await _context.Merchants.FirstOrDefaultAsync(x => x.Id == appUser.Id);
    }

}
