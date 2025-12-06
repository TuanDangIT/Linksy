using Linksy.API.API;
using Linksy.Application.Statistics.Analytics;
using Linksy.Application.Statistics.Features.GetBarodeEngagementAnalytics;
using Linksy.Application.Statistics.Features.GetCampaignCountsForUmtParameter;
using Linksy.Application.Statistics.Features.GetLandingPageEngagementAnalytics;
using Linksy.Application.Statistics.Features.GetLandingPageViewAnalytics;
using Linksy.Application.Statistics.Features.GetMediumCountsForUmtParameter;
using Linksy.Application.Statistics.Features.GetQrCodeEngagementAnalytics;
using Linksy.Application.Statistics.Features.GetSourceCountsForUmtParameter;
using Linksy.Application.Statistics.Features.GetUmtParameterEngagementAnalytics;
using Linksy.Application.Statistics.Features.GetUrlEngagementAnalytics;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Linksy.API.Controllers
{
    public class AnalyticsController : BaseController
    {
        public AnalyticsController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet("engagements/urls/{urlId:int}")]
        public async Task<ActionResult<ApiResponse<AnalyticsResponse>>> GetUrlEngagementAnalyticsAsync([FromRoute] int urlId, [FromQuery] GetUrlEngagementAnalytics query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query with { UrlId = urlId }, cancellationToken);
            return Ok(result);
        }

        [HttpGet("engagements/barcodes/{barcodeId:int}")]
        public async Task<ActionResult<ApiResponse<AnalyticsResponse>>> GetBarcodeEngagementAnalyticsAsync([FromRoute] int barcodeId, [FromQuery] GetBarcodeEngagementAnalytics query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query with { BarcodeId = barcodeId }, cancellationToken);
            return Ok(result);
        }

        [HttpGet("engagements/qrcodes/{qrCodeId:int}")]
        public async Task<ActionResult<ApiResponse<AnalyticsResponse>>> GetQrCodeEngagementAnalyticsAsync([FromRoute] int qrCodeId, [FromQuery] GetQrCodeEngagementAnalytics query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query with { QrCodeId = qrCodeId }, cancellationToken);
            return Ok(result);
        }

        [HttpGet("engagements/landingpages/{landingPageId:int}")]
        public async Task<ActionResult<ApiResponse<AnalyticsResponse>>> GetLandingPageEngagementAnalyticsAsync([FromRoute] int landingPageId, [FromQuery] GetLandingPageEngagementAnalytics query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query with { LandingPageId = landingPageId }, cancellationToken);
            return Ok(result);
        }

        [HttpGet("engagements/umtparameters/{umtParameterId:int}")]
        public async Task<ActionResult<ApiResponse<AnalyticsResponse>>> GetUmtParameterEngagementAnalyticsAsync([FromRoute] int umtParameterId, [FromQuery] GetUmtParameterEngagementAnalytics query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query with { UmtParameterId = umtParameterId }, cancellationToken);
            return Ok(result);
        }


        [HttpGet("counts/urls/{urlId:int}/umtparameters/campaigns")]
        public async Task<ActionResult<ApiResponse<GetCampaignCountsForUmtParameterResponse>>> GetUmtCampaignCountsForUrlAsync([FromRoute] int urlId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCampaignCountsForUmtParameter(urlId), cancellationToken);
            return Ok(result);
        }

        [HttpGet("counts/urls/{urlId:int}/umtparameters/mediums")]
        public async Task<ActionResult<ApiResponse<GetMediumCountsForUmtParameterResponse>>> GetUmtMediumCountsForUrlAsync([FromRoute] int urlId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetMediumCountsForUmtParameter(urlId), cancellationToken);
            return Ok(result);
        }

        [HttpGet("counts/urls/{urlId:int}/umtparameters/sources")]
        public async Task<ActionResult<ApiResponse<GetSourceCountsForUmtParameterResponse>>> GetUmtSourceCountsForUrlAsync([FromRoute] int urlId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetSourceCountsForUmtParameter(urlId), cancellationToken);
            return Ok(result);
        }


        [HttpGet("views/landingpages/{landingPageId:int}")]
        public async Task<ActionResult<ApiResponse<AnalyticsResponse>>> GetLandingPageViewAnalyticsAsync([FromRoute] int landingPageId, [FromQuery] GetLandingPageViewAnalytics query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query with { LandingPageId = landingPageId }, cancellationToken);
            return Ok(result);
        }
    }
}
