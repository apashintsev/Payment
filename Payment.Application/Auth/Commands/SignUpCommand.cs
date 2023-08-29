using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Payment.Application.Auth.Dtos;
using Payment.Domain.Config;
using Payment.Domain.Entities;
using Payment.Domain.Exceptions;
using Payment.Infrastructure;
using Payment.Infrastructure.Services;

namespace Payment.Application.Auth.Commands;

public record SignUpCommand(string Email, string Phone, string Password, string Referral) : IRequest<AuthResultVm>;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, AuthResultVm>
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;
    private readonly ApplicationDbContext _context;
    private ILogger<SignUpCommandHandler> _logger;

    public SignUpCommandHandler(IAuthService authService, ITokenService tokenService, ILogger<SignUpCommandHandler> logger, ApplicationDbContext context)
    {
        _authService = authService;
        _tokenService = tokenService;
        _logger = logger;
        _context = context;
    }

    public async Task<AuthResultVm> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var exsistedEmail = await _context.Merchants.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (exsistedEmail is not null)
        {
            var ex = new UserAllreadyExsistsException(request.Email);
            _logger.LogError(ex.Message);
            throw ex;
        }
        var exsistedPhone = await _context.Merchants.FirstOrDefaultAsync(x => x.Phone == request.Phone);
        if (exsistedPhone is not null)
        {
            var ex = new UserAllreadyExsistsException(request.Phone);
            _logger.LogError(ex.Message);
            throw ex;
        }

        var user = new Merchant(request.Email, request.Phone);
        var result = await _authService.CreateUser(user, request.Password);

        if (!string.IsNullOrWhiteSpace(request.Referral) && result)
        {
            var owner = await _context.Merchants.Include(x => x.ReferredUsers).FirstOrDefaultAsync(x => x.ReferralLink == request.Referral.Trim());
            if (owner is not null)
            {
                var referral = new Referral(owner, user);
                owner.ReferredUsers.Add(referral);
            }
            else
            {
                _logger.LogWarning("Invalid ref url");
            }
        }

        var token = await _tokenService.CreateToken(user);

        var nickname = $"{user.FirstName} {user.LastName}";

        return new AuthResultVm()
        {
            JwtToken = token,
            Nickname = string.IsNullOrWhiteSpace(nickname) ? user.Email : nickname,
            UserId = user.Id.ToString()
        };
    }
}


public class SignUpByEmailCommandValidator : AbstractValidator<SignUpCommand>
{
    private readonly PasswordSettings _passwordSettings;

    public SignUpByEmailCommandValidator(IOptions<PasswordSettings> passwordSettings)
    {
        _passwordSettings = passwordSettings.Value;

        RuleFor(m => m.Email).NotEmpty().WithMessage("Email required");
        RuleFor(m => m.Email).Custom((value, context) =>
        {
            if (!Merchant.IsValidEmail(value))
                context.AddFailure("Invalid Email format");
        });

        RuleFor(m => m.Phone).NotEmpty().WithMessage("Phone required");
        RuleFor(m => m.Phone).Custom((value, context) =>
        {
            if (!Merchant.IsValidPhone(Merchant.CleanPhoneNumber(value)))
                context.AddFailure("Invalid Phone format");
        });

    }
}
