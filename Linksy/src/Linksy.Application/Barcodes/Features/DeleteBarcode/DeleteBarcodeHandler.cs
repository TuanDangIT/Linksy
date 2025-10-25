using Linksy.Application.Abstractions;
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
        private readonly ILogger<DeleteBarcodeHandler> _logger;

        public DeleteBarcodeHandler(IBarcodeRepository barcodeRepository, IContextService contextService, ILogger<DeleteBarcodeHandler> logger)
        {
            _barcodeRepository = barcodeRepository;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task Handle(DeleteBarcode request, CancellationToken cancellationToken)
        {
            await _barcodeRepository.DeleteAsync(request.BarcodeId, request.IncludeUrlInDeletion, cancellationToken);
            _logger.LogInformation("Barcode with ID: {BarcodeId} was deletedwith include URL in deletion: {IncludeUrlInDeletion} by user with ID {UserId}.", 
                request.BarcodeId, request.IncludeUrlInDeletion, _contextService.Identity!.Id);
        }
    }
}
