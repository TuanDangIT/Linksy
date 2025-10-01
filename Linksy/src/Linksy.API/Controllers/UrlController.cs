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
    }
}
