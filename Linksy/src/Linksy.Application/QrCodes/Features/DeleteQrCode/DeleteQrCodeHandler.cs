using Linksy.Application.Abstractions;
using Linksy.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.QrCodes.Features.DeleteQrCode
{
    internal class DeleteQrCodeHandler : ICommandHandler<DeleteQrCode>
    {
        private readonly IQrCodeRepository _qrCodeRepository;
        private readonly IContextService _contextService;
        private readonly ILogger<DeleteQrCodeHandler> _logger;

        public DeleteQrCodeHandler(IQrCodeRepository qrCodeRepository, IContextService contextService, ILogger<DeleteQrCodeHandler> logger)
        {
            _qrCodeRepository = qrCodeRepository;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task Handle(DeleteQrCode request, CancellationToken cancellationToken)
        {
            await _qrCodeRepository.DeleteAsync(request.QrCodeId, request.IncludeUrlInDeletion, cancellationToken);
            _logger.LogInformation("QR Code with ID: {QrCodeId} was deleted with include URL in deletion: {IncludeUrlInDeletion} by user with ID {UserId}.", 
                request.QrCodeId, request.IncludeUrlInDeletion, _contextService.Identity!.Id);
        }
    }
}
