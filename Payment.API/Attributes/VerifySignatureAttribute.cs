using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using MediatR;
using Payment.Application.Merchants;

namespace Payment.API.Attributes;

public class VerifySignatureAttribute : ActionFilterAttribute
{
    private readonly IMediator _mediator;

    public VerifySignatureAttribute(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task OnActionExecutionAsync( ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var headers = context.HttpContext.Request.Headers;
        var body = "";

        using (var reader = new StreamReader(context.HttpContext.Request.Body))
        {
            body = await reader.ReadToEndAsync();
        }

        if (!headers.ContainsKey("X-Processing-Key") || !headers.ContainsKey("X-Processing-Signature"))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var publicKey = headers["X-Processing-Key"].ToString();
        var signature = headers["X-Processing-Signature"].ToString();

        var merchant = await _mediator.Send(new GetMerchantByPublicKeyQuery(publicKey));

        if (merchant == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var computedSignature = ComputeSignature(body, merchant.SecretKey);

        if (computedSignature != signature)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
    }

    private string ComputeSignature(string data, string key)
    {
        using var hmac = new HMACSHA512(Encoding.ASCII.GetBytes(key));
        var hash = hmac.ComputeHash(Encoding.ASCII.GetBytes(data));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}
