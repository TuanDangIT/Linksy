using Linksy.API.API;
using Linksy.Application.UmtParameters.Features.AddQrCodeToUmtParameter;
using Linksy.Application.UmtParameters.Features.AddUmtParameterToUrl;
using Linksy.Application.UmtParameters.Features.DeleteUmtParameter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Linksy.API.Controllers
{
    public class UmtParametersController : BaseController
    {
        public UmtParametersController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("/api/v{v:apiVersion}/urls/{urlId}" + "/[controller]")]
        public async Task<ActionResult<ApiResponse<AddUmtParameterToUrlResponse>>> AddUmtParameterToUrl([FromRoute] int urlId, [FromBody] AddUmtParameterToUrl command, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(command with { UrlId = urlId }, cancellationToken));

        [HttpPost("{umtParameterId:int}/qrcode")]
        public async Task<ActionResult<ApiResponse<AddQrCodeToUmtParameterResponse>>> GenerateQrCodeForUmtParameter([FromRoute] int umtParameterId,
            [FromBody] AddQrCodeToUmtParameter command, CancellationToken cancellationToken)
        {
            command = command with { UmtParameterId = umtParameterId };
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{umtParameterId:int}")]
        public async Task<ActionResult> DeleteUmtParameter([FromRoute] int umtParameterId, CancellationToken cancellationToken)
        {
            var command = new DeleteUmtParameter(umtParameterId);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
