using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Payment.Application.Auth.Dtos;
using Payment.Application.Auth.Commands;
using Payment.Application.Common;

namespace Payment.API.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public partial class AuthController : ControllerBase
{
    public record UserSignInDto(string Email, string Password);
    /// <summary>
    /// Логин по Email и паролю
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<AuthResultVm>), 200)]
    public async Task<IActionResult> Login([FromServices] IMediator mediator, [FromBody] UserSignInDto dto)
    {
        var cmd = new LoginCommand(dto.Email, dto.Password);
        var response = new BaseResponse<AuthResultVm>(await mediator.Send(cmd));
        return Ok(response);
    }

    public record UserSignUpDto(string Email, string Phone, string Password, string Referral);
    /// <summary>
    /// Регистрация с использованием почты и телефона
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<AuthResultVm>), 200)]
    public async Task<IActionResult> SignUp([FromServices] IMediator mediator, [FromBody] UserSignUpDto dto)
    {
        var cmd = new SignUpCommand(dto.Email, dto.Phone, dto.Password, dto.Referral);
        var response = new BaseResponse<AuthResultVm>(await mediator.Send(cmd));
        return Ok(response);
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> TestAuth()
    {
        string nameId = User.FindFirst(JwtRegisteredClaimNames.NameId)?.Value;
        return Ok("Authorized: " + nameId);
    }
}
