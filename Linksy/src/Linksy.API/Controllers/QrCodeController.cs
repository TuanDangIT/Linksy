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

        [HttpGet("{qrCodeId:int}")]
        public async Task<FileStreamResult> DownloadQrCode([FromRoute] int qrCodeId, CancellationToken cancellationToken)
            => await _mediator.Send(new DownloadQrCode(qrCodeId), cancellationToken);
    }
}
