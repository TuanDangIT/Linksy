using Linksy.API.API;
using Linksy.Application.QrCodes.Features.BrowseQrCodes;
using Linksy.Application.QrCodes.Features.CreateQrCode;
using Linksy.Application.QrCodes.Features.DeleteQrCode;
using Linksy.Application.QrCodes.Features.DownloadQrCode;
using Linksy.Application.QrCodes.Features.GetQrCode;
using Linksy.Application.QrCodes.Features.ModifyTags;
using Linksy.Application.Shared.Pagination;
using Linksy.Domain.Entities.ScanCode;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linksy.API.Controllers
{
    [Authorize]
    public class QrCodesController : BaseController
    {
        public QrCodesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<BrowseQrCodeResponse>>> BrowseQrCodes([FromQuery]BrowseQrCodes query, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(query, cancellationToken));

        [HttpGet("{qrCodeId:int}")]
        public async Task<ActionResult<ApiResponse<GetQrCodeResponse>>> GetQrCodeById([FromRoute] int qrCodeId, CancellationToken cancellationToken)
            => OkOrNotFound(await _mediator.Send(new GetQrCode(qrCodeId), cancellationToken), nameof(QrCode));

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CreateQrCodeResponse>>> CreateQrCode([FromBody] CreateQrCode command, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(command, cancellationToken));

        [HttpGet("{qrCodeId:int}/download")]
        public async Task<FileStreamResult> DownloadQrCode([FromRoute] int qrCodeId, CancellationToken cancellationToken)
            => await _mediator.Send(new DownloadQrCode(qrCodeId), cancellationToken);

        [HttpDelete("{qrCodeId:int}")]
        public async Task<ActionResult> DeleteQrCode([FromRoute] int qrCodeId, [FromQuery] bool includeUrlInDeletion, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteQrCode(qrCodeId, includeUrlInDeletion), cancellationToken);
            return NoContent();
        }

        [HttpPut("{qrCodeId:int}/tags")]
        public async Task<ActionResult> UpdateQrCodeTags([FromRoute] int qrCodeId, [FromBody] IEnumerable<string> tags, CancellationToken cancellationToken)
        {
            await _mediator.Send(new ModifyTags(qrCodeId, tags), cancellationToken);
            return NoContent();
        }
    }
}
