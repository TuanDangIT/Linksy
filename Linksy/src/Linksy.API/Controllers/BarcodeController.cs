﻿using Linksy.API.API;
using Linksy.Application.Barcodes.Features.CreateBarcode;
using Linksy.Application.Barcodes.Features.DownloadBarcode;
using Linksy.Application.QrCodes.Features.CreateQrCode;
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

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CreateBarcodeResponse>>> CreateBarcode([FromBody] CreateBarcode command, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(command, cancellationToken));

        [HttpGet("{barcodeId:int}")]
        public async Task<FileStreamResult> DownloadBarcode([FromRoute] int barcodeId, CancellationToken cancellationToken)
            => await _mediator.Send(new DownloadBarcode(barcodeId), cancellationToken);
    }
}
