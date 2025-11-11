using Linksy.Application.Abstractions;
using Linksy.Application.QrCodes.Exceptions;
using Linksy.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.QrCodes.Features.ModifyTags
{
    internal class ModifyTagsHandler : ICommandHandler<ModifyTags>
    {
        private readonly IQrCodeRepository _qrCodeRepository;
        private readonly IContextService _contextService;
        private readonly ILogger<ModifyTagsHandler> _logger;

        public ModifyTagsHandler(IQrCodeRepository qrCodeRepository, IContextService contextService, ILogger<ModifyTagsHandler> logger)
        {
            _qrCodeRepository = qrCodeRepository;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task Handle(ModifyTags request, CancellationToken cancellationToken)
        {
            var qrCode = await _qrCodeRepository.GetByIdAsync(request.QrCodeId, cancellationToken) ??
                throw new QrCodeNotFoundException(request.QrCodeId);
            qrCode.UpdateTags(request.Tags);
            _logger.LogInformation("Tags for QR Code with ID: {QrCodeId} were modified by user with ID: {UserId}.", request.QrCodeId, _contextService.Identity!.Id);
            await _qrCodeRepository.UpdateAsync(cancellationToken);
        }
    }
}
