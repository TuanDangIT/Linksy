using Linksy.Application.Barcodes.Features.DownloadBarcode;
using Linksy.Application.QrCodes.Features.DownloadQrCode;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linksy.API.Controllers
{
    [Authorize]
    public class BarcodeController : BaseController
    {
        public BarcodeController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("{barcodeId:int}")]
        public async Task<FileStreamResult> DownloadBarcode([FromRoute] int barcodeId, CancellationToken cancellationToken)
            => await _mediator.Send(new DownloadBarcode(barcodeId), cancellationToken);
    }
}
