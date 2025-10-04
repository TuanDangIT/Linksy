using Linksy.API.API;
using Linksy.Application.Urls.DTO;
using Linksy.Application.Urls.Features.BrowseUrls;
using Linksy.Application.Urls.Features.RedirectToOriginalUrl;
using Linksy.Application.Urls.Features.ShortenUrl;
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
        public async Task<IActionResult> ShortentUrl([FromBody] ShortenUrl command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("{code}")]
        public async Task<IActionResult> RedirectToOriginalUrl([FromRoute] string code)
        {
            var result = await _mediator.Send(new RedirectToOriginalUrl(code));
            return Redirect(result.OriginalUrl);
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<BrowseUrlDto>>>> BrowseUrls()
        {
            var result = await _mediator.Send(new BrowseUrls());
            return Ok(new ApiResponse<IEnumerable<BrowseUrlDto>>(result));
        }
    }
}
