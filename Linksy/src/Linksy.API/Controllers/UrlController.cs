using Linksy.API.API;
using Linksy.Application.Urls.Features.BrowseUrls;
using Linksy.Application.Urls.Features.GetUrl;
using Linksy.Application.Urls.Features.RedirectToOriginalUrl;
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
        public async Task<ActionResult<ApiResponse<ShortenedUrlDto>>> ShortentUrl([FromBody] ShortenUrl command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("/{code}")]
        public async Task<ActionResult<ApiResponse<object>>> RedirectToOriginalUrl([FromRoute] string code)
        {
            var result = await _mediator.Send(new RedirectToOriginalUrl(code));
            return Request.Headers.ContainsKey("Referer") && Request.Headers.Referer.ToString().Contains("swagger") ?
                Ok<object>(new { redirectUrl = result.OriginalUrl, additionalMessage = "Response for swagger only." }) :
                Redirect(result.OriginalUrl);
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<BrowseUrlDto>>>> BrowseUrls()
        {
            var result = await _mediator.Send(new BrowseUrls());
            return Ok(result);
        }
        [HttpGet("{urlId:int}")]
        public async Task<ActionResult<ApiResponse<GetUrlDto>>> GetUrlById([FromRoute] int urlId)
            => OkOrNotFound(await _mediator.Send(new GetUrl(urlId)), nameof(Url), urlId);
    }
}
