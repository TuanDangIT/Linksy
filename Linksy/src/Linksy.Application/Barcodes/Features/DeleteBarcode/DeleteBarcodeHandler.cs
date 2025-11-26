using Linksy.Application.Abstractions;
using Linksy.Application.Shared.BlobStorage;
using Linksy.Application.Shared.Configuration;
using Linksy.Application.Shared.ScanCodes;
using Linksy.Domain.Entities.ScanCode;
using Linksy.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Barcodes.Features.DeleteBarcode
{
    internal class DeleteBarcodeHandler : ICommandHandler<DeleteBarcode>
    {
        private readonly IBarcodeRepository _barcodeRepository;
        private readonly IContextService _contextService;
        private readonly IScanCodeService _scanCodeService;
        private readonly LinksyConfig _linksyConfig;
        private readonly ILogger<DeleteBarcodeHandler> _logger;

        public DeleteBarcodeHandler(IBarcodeRepository barcodeRepository, IContextService contextService, IScanCodeService scanCodeService, 
            LinksyConfig linksyConfig, ILogger<DeleteBarcodeHandler> logger)
        {
            _barcodeRepository = barcodeRepository;
            _contextService = contextService;
            _scanCodeService = scanCodeService;
            _linksyConfig = linksyConfig;
            _logger = logger;
        }
        public async Task Handle(DeleteBarcode request, CancellationToken cancellationToken)
        {
            var barcode = await _barcodeRepository.GetByIdAsync(request.BarcodeId, cancellationToken);
            if(barcode is null)
            {
                return;
            }   
            var fileName = barcode.ScanCodeImage.FileName;
            var userId = _contextService.Identity!.Id;
            await _scanCodeService.DeleteAsync(fileName, userId, cancellationToken);
            await _barcodeRepository.DeleteAsync(request.BarcodeId, request.IncludeUrlInDeletion);
            _logger.LogInformation("Barcode with ID: {BarcodeId} was deletedwith include URL in deletion: {IncludeUrlInDeletion} by user with ID {UserId}.", 
                request.BarcodeId, request.IncludeUrlInDeletion, userId);
        }
    }
}
