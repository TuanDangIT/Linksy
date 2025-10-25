using Linksy.API.API;
using Linksy.Application.QrCodes.Features.CreateQrCode;
using Linksy.Application.QrCodes.Features.DeleteQrCode;
using Linksy.Application.QrCodes.Features.DownloadQrCode;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linksy.API.Controllers
{
    [Authorize]
    public class QrCodeController : BaseController
    {
        public QrCodeController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CreateQrCodeResponse>>> CreateQrCode([FromBody]CreateQrCode command, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(command, cancellationToken));

        [HttpGet("{qrCodeId:int}")]
        public async Task<FileStreamResult> DownloadQrCode([FromRoute] int qrCodeId, CancellationToken cancellationToken)
            => await _mediator.Send(new DownloadQrCode(qrCodeId), cancellationToken);

        [HttpDelete("{qrCodeId:int}")]
        public async Task<ActionResult> DeleteQrCode([FromRoute] int qrCodeId, [FromQuery] bool includeUrlInDeletion, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteQrCode(qrCodeId, includeUrlInDeletion), cancellationToken);
            return NoContent();
        }
    }
}
