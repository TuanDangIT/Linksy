using Linksy.Application.Abstractions;
using Linksy.Application.Barcodes.Exceptions;
using Linksy.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Barcodes.Features.ModifyTags
{
    internal class ModifyTagsHandler : ICommandHandler<ModifyTags>
    {
        private readonly IBarcodeRepository _barcodeRepository;
        private readonly IContextService _contextService;
        private readonly ILogger<ModifyTagsHandler> _logger;

        public ModifyTagsHandler(IBarcodeRepository barcodeRepository, IContextService contextService, ILogger<ModifyTagsHandler> logger)
        {
            _barcodeRepository = barcodeRepository;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task Handle(ModifyTags request, CancellationToken cancellationToken)
        {
            var barcode = await _barcodeRepository.GetByIdAsync(request.BarcodeId, cancellationToken) ?? throw new BarcodeNotFoundException(request.BarcodeId);
            barcode.UpdateTags(request.Tags);
            _logger.LogInformation("Tags for Barcode with ID: {BarcodeId} were modified by user with ID: {UserId}.", request.BarcodeId, _contextService.Identity!.Id);
            await _barcodeRepository.UpdateAsync(cancellationToken);
        }
    }
}
