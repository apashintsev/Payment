using MediatR;
using Microsoft.AspNetCore.Mvc;
using NBitcoin.Secp256k1;
using Payment.API.Attributes;
using Payment.Application.Addresses;
using Payment.Application.Addresses.Dto;
using Payment.Application.Common;

namespace Payment.API.Controllers;

[Route("[controller]/[action]")]
[ApiController]
[TypeFilter(typeof(VerifySignatureAttribute))]
public class ApiController : Controller
{
    public record TakeAddressRequest(string ForeignId, string Currency, string ConvertTo);

    /// <summary>
    /// Take address for depositing crypto and (it depends on specified params) exchange from crypto to fiat on-the-fly.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="foreignId">Your info for this address, will returned as reference in Address responses, example: user-id:2048</param>
    /// <param name="currency">ISO of currency to receive funds in, example: BTC</param>
    /// <param name="convertTo">If you need auto exchange all incoming funds for example to EUR, specify this param as EUR or any other supported currency ISO, to see list of pairs see previous method.</param>
    /// <returns></returns>
    [HttpPost]
    [Route("v2/addresses/take")]
    [ProducesResponseType(typeof(BaseResponse<TakeAddressResponseVm>), 200)]
    public async Task<IActionResult> TakeAddress([FromServices] IMediator mediator, [FromBody] TakeAddressRequest request)
    {
        var cmd = new TakeAddressCommand(HttpContext.Request.Headers["X-Processing-Key"].ToString(), request.ForeignId,request.Currency,request.ConvertTo);
        var response = new BaseResponse<TakeAddressResponseVm>(await mediator.Send(cmd));
        return Ok(response);
    }


}
