﻿using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Payment.Application.Auth.Dtos;
using Payment.Domain.Entities;
using Payment.Domain.Exceptions;
using Payment.Infrastructure.Services;

namespace Payment.Application.Auth.Commands;

public record LoginCommand(string Email, string Password) : IRequest<AuthResultVm>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResultVm>
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;
    private ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(IAuthService authService, ITokenService tokenService, ILogger<LoginCommandHandler> logger)
    {
        _tokenService = tokenService;
        _authService = authService;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<AuthResultVm> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _authService.FindByEmailAsync(request.Email);

        if (user is null)
        {
            var ex = new UserNotFoundException(request.Email);
            _logger.LogError(ex.Message);
            throw ex;
        }

        var isPasswordCorrect = await _authService.CheckPasswordSignInAsync(user, request.Password);

        if (!isPasswordCorrect)
        {
            var ex = new IncorrectPasswordException(request.Email);
            _logger.LogError(ex.Message);
            throw ex;
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


    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(m => m.Email).NotEmpty().WithMessage("Email required");
            RuleFor(m => m.Email).Custom((value, context) =>
            {
                if (!Merchant.IsValidEmail(value))
                    context.AddFailure("Invalid Email format");
            });
        }
    }
}
