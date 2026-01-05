using Linksy.API.API;
using Linksy.Application.Barcodes.Features.BrowseBarcodes;
using Linksy.Application.Barcodes.Features.CreateBarcode;
using Linksy.Application.Barcodes.Features.DeleteBarcode;
using Linksy.Application.Barcodes.Features.DownloadBarcode;
using Linksy.Application.Barcodes.Features.GetBarcode;
using Linksy.Application.Barcodes.Features.ModifyTags;
using Linksy.Application.QrCodes.Features.BrowseQrCodes;
using Linksy.Application.QrCodes.Features.CreateQrCode;
using Linksy.Application.QrCodes.Features.DownloadQrCode;
using Linksy.Domain.Entities.ScanCode;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linksy.API.Controllers
{
    [Authorize]
    public class BarcodesController : BaseController
    {
        public BarcodesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<BrowseBarcodesResponse>>> BrowseBarcodes([FromQuery] BrowseBarcodes query, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(query, cancellationToken));

        [HttpGet("{barcodeId:int}")]
        public async Task<ActionResult<ApiResponse<GetBarcodeResponse>>> GetBarcodeById([FromRoute] int barcodeId, CancellationToken cancellationToken)
            => OkOrNotFound(await _mediator.Send(new GetBarcode(barcodeId), cancellationToken), $"Barcode with ID: {barcodeId} was not found.");

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CreateBarcodeResponse>>> CreateBarcode([FromBody] CreateBarcode command, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(command, cancellationToken));

        [HttpGet("{barcodeId:int}/download")]
        public async Task<FileStreamResult> DownloadBarcode([FromRoute] int barcodeId, CancellationToken cancellationToken)
            => await _mediator.Send(new DownloadBarcode(barcodeId), cancellationToken);

        [HttpDelete("{barcodeId:int}")]
        public async Task<ActionResult> DeleteBarcode([FromRoute] int barcodeId, [FromQuery] bool includeUrlInDeletion, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteBarcode(barcodeId, includeUrlInDeletion), cancellationToken);
            return NoContent();
        }

        [HttpPut("{barcodeId:int}/tags")]
        public async Task<ActionResult> UpdateBarcodeTags([FromRoute] int barcodeId, [FromBody] IEnumerable<string> tags, CancellationToken cancellationToken)
        {
            await _mediator.Send(new ModifyTags(barcodeId, tags), cancellationToken);
            return NoContent();
        }
    }
}
