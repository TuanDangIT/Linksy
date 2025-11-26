using Linksy.API.API;
using Linksy.Application.LandingPages.Features.AddLandingPageEngagement;
using Linksy.Application.LandingPages.Features.BrowseLandingPages;
using Linksy.Application.LandingPages.Features.CreateLandingPage;
using Linksy.Application.LandingPages.Features.GetLandingPage;
using Linksy.Application.LandingPages.Features.GetPublishedLandingPage;
using Linksy.Application.LandingPages.Features.PublishLandingPage;
using Linksy.Domain.Entities.LandingPage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Linksy.API.Controllers
{
    public class LandingPagesController : BaseController
    {
        public LandingPagesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<BrowseLandingPagesResponse>>> BrowseLandingPages([FromQuery] BrowseLandingPages query, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(query, cancellationToken));

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CreateLandingPageResponse>>> CreateLandingPage([FromForm] CreateLandingPage command, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(command, cancellationToken));

        [HttpGet("{landingPageId:int}")]
        public async Task<ActionResult<ApiResponse<GetLandingPageResponse>>> GetLandingPage([FromRoute]int landingPageId, CancellationToken cancellationToken)
            => OkOrNotFound(await _mediator.Send(new GetLandingPage(landingPageId), cancellationToken), nameof(LandingPage), landingPageId);

        [AllowAnonymous]
        [HttpGet("/lp/{code}")]
        public async Task<ActionResult<ApiResponse<GetPublishedLandingPageResponse>>> GetPublishedLandingPage([FromRoute] string code, CancellationToken cancellationToken)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            return OkOrNotFound(await _mediator.Send(new GetPublishedLandingPage(code, ipAddress), cancellationToken), nameof(LandingPage), code);
        }

        [AllowAnonymous]
        [HttpPatch("{landingPageId:int}/engagements")]
        public async Task<ActionResult> TrackLandingPageEngagement([FromRoute] int landingPageId, [FromBody] int landingPageItemId, CancellationToken cancellationToken)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            await _mediator.Send(new AddLandingPageEngagement(landingPageId, landingPageItemId, ipAddress), cancellationToken);
            return Ok();
        }

        [HttpPost("{landingPageId:int}/publish")]   
        public async Task<ActionResult> PublishLandingPage([FromRoute] int landingPageId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new PublishLandingPage(landingPageId, true), cancellationToken);
            return Ok();
        }

        [HttpPost("{landingPageId:int}/unpublish")]
        public async Task<ActionResult> UnpublishLandingPage([FromRoute] int landingPageId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new PublishLandingPage(landingPageId, false), cancellationToken);
            return Ok();
        }

        private ActionResult<ApiResponse<TResponse>> OkOrNotFound<TResponse>(TResponse? model, string entityName, string code)
        {
            if (model is not null)
            {
                return Ok(model);
            }
            return NotFound(new ProblemDetails()
            {
                Type = _notFoundTypeUrl,
                Title = $"{entityName} with code: {code} was not found.",
                Status = (int)HttpStatusCode.NotFound
            });
        }
    }
}
