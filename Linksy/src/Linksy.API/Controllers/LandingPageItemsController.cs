using Linksy.API.API;
using Linksy.Application.LandingPageItems.Features.AddImageLandingPageItem;
using Linksy.Application.LandingPageItems.Features.AddTextLandingPageItem;
using Linksy.Application.LandingPageItems.Features.AddUrlLandingPageItem;
using Linksy.Application.LandingPageItems.Features.AddYouTubeLandingPageItem;
using Linksy.Application.LandingPageItems.Features.DeleteLandingPageItem;
using Linksy.Application.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Linksy.API.Controllers
{
    [Route("/api/v{v:apiVersion}/LandingPages/{landingPageId:int}/[controller]")]
    public class LandingPageItemsController : BaseController
    {
        public LandingPageItemsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("images")]
        public async Task<ActionResult<ApiResponse<AddImageLandingPageItemResponse>>> AddLandingPageItem([FromRoute] int landingPageId, [FromForm] AddImageLandingPageItem command, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(command with { LandingPageId = landingPageId }, cancellationToken));

        [HttpPost("youtubes")]
        public async Task<ActionResult<ApiResponse<AddLandingPageResponse>>> AddYouTubeLandingPageItem([FromRoute] int landingPageId, [FromBody] AddYouTubeLandingPageItem command, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(command with { LandingPageId = landingPageId }, cancellationToken));

        [HttpPost("texts")]
        public async Task<ActionResult<ApiResponse<AddLandingPageResponse>>> AddTextLandingPageItem([FromRoute] int landingPageId, [FromBody] AddTextLandingPageItem command, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(command with { LandingPageId = landingPageId }, cancellationToken));

        [HttpPost("urls")]
        public async Task<ActionResult<ApiResponse<AddLandingPageResponse>>> AddUrlLandingPageItem([FromRoute] int landingPageId, [FromBody] AddUrlLandingPageItem command, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(command with { LandingPageId = landingPageId }, cancellationToken));

        [HttpDelete("/api/v{v:apiVersion}/[controller]/{landingPageItemId:int}")]
        public async Task<ActionResult> DeleteLandingPageItem([FromRoute] int landingPageItemId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteLandingPageItem(landingPageItemId), cancellationToken);
            return NoContent();
        }
    }
}
