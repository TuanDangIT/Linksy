using Linksy.API.API;
using Linksy.Application.Shared.DTO;
using Linksy.Application.Urls.Features.AddBarcode;
using Linksy.Application.Urls.Features.AddQrCode;
using Linksy.Application.Urls.Features.BrowseUrls;
using Linksy.Application.Urls.Features.ChangeOriginalUrl;
using Linksy.Application.Urls.Features.DeleteUrl;
using Linksy.Application.Urls.Features.GetUrl;
using Linksy.Application.Urls.Features.RedirectToOriginalUrl;
using Linksy.Application.Urls.Features.SetActiveStatus;
using Linksy.Application.Urls.Features.ShortenUrl;
using Linksy.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linksy.API.Controllers
{
    [Authorize]
    public class UrlController : BaseController
    {
        public UrlController(IMediator mediator) : base(mediator)
        {
        }
        [HttpPost()]
        public async Task<ActionResult<ApiResponse<ShortenedUrlResponse>>> ShortentUrl([FromBody] ShortenUrl command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("/{code}")]
        public async Task<ActionResult<ApiResponse<object>>> RedirectToOriginalUrl([FromRoute] string code, [FromQuery] string? umtSource, 
            [FromQuery] string? umtMedium, [FromQuery] string? umtCampaign, CancellationToken cancellationToken)
        {
            RedirectToOriginalUrlResponse result;
            if (umtSource is not null || umtMedium is not null || umtCampaign is not null)
            {
                result = await _mediator.Send(new RedirectToOriginalUrl(code, new UmtParameterDto(umtSource, umtMedium, umtCampaign)), cancellationToken);
            }
            else
            {
                result = await _mediator.Send(new RedirectToOriginalUrl(code, null), cancellationToken);
            }
            return Request.Headers.ContainsKey("Referer") && Request.Headers.Referer.ToString().Contains("swagger") ?
                Ok<object>(new { redirectUrl = result.OriginalUrl, additionalMessage = "Response for swagger only." }) :
                Redirect(result.OriginalUrl);
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<BrowseUrlResponse>>> BrowseUrls(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new BrowseUrls(), cancellationToken);
            return Ok(result);
        }
        [HttpGet("{urlId:int}")]
        public async Task<ActionResult<ApiResponse<GetUrlResponse>>> GetUrlById([FromRoute] int urlId, CancellationToken cancellationToken)
            => OkOrNotFound(await _mediator.Send(new GetUrl(urlId), cancellationToken), nameof(Url), urlId);
        [HttpDelete("{urlId:int}")]
        public async Task<ActionResult> DeleteUrlById([FromRoute] int urlId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteUrl(urlId), cancellationToken);
            return NoContent();
        }
        [HttpPatch("{urlId:int}/original-url")]
        public async Task<ActionResult> ChangeOriginalUrl([FromRoute] int urlId, [FromBody] ChangeOriginalUrl command, CancellationToken cancellationToken)
        {
            command = command with { UrlId = urlId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
        [HttpPatch("{urlId:int}/activate")]
        public async Task<ActionResult> ActivateUrl([FromRoute] int urlId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new SetActiveStatus(urlId, true), cancellationToken);
            return NoContent();
        }
        [HttpPatch("{urlId:int}/deactivate")]
        public async Task<ActionResult> DeactivateUrl([FromRoute] int urlId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new SetActiveStatus(urlId, true), cancellationToken);
            return NoContent();
        }
        [HttpPost("{urlId:int}/qrcode")]
        public async Task<ActionResult<ApiResponse<AddQrCodeResponse>>> GenerateQrCodeForUrl([FromRoute] int urlId, [FromBody] AddQrCode command, CancellationToken cancellationToken)
        {
            command = command with { UrlId = urlId };
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        [HttpPost("{urlId:int}/barcode")]
        public async Task<ActionResult<ApiResponse<AddBarcodeResponse>>> GenerateBarcodeForUrl([FromRoute] int urlId, [FromBody] AddBarcode command, CancellationToken cancellationToken)
        {
            command = command with { UrlId = urlId };
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
