using Linksy.Application.Abstractions;
using Linksy.Application.Shared.BlobStorage;
using Linksy.Application.Shared.Configuration;
using Linksy.Application.Shared.ScanCodes;
using Linksy.Application.UmtParameters.Exceptions;
using Linksy.Application.Urls.Exceptions;
using Linksy.Domain.Entities.ScanCode;
using Linksy.Domain.Repositories;
using Linksy.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.UmtParameters.Features.AddQrCodeToUmtParameter
{
    internal class AddQrCodeToUmtParameterHandler : ICommandHandler<AddQrCodeToUmtParameter, AddQrCodeToUmtParameterResponse>
    {
        private readonly IUmtParameterRepository _umtParameterRepository;
        private readonly IScanCodeService _scanCodeService;
        private readonly LinksyConfig _linksyConfig;
        private readonly IContextService _contextService;
        private readonly ILogger<AddQrCodeToUmtParameterHandler> _logger;

        public AddQrCodeToUmtParameterHandler(IUmtParameterRepository umtParameterRepository, IScanCodeService scanCodeService, 
            LinksyConfig linksyConfig, IContextService contextService, ILogger<AddQrCodeToUmtParameterHandler> logger)
        {
            _umtParameterRepository = umtParameterRepository;
            _scanCodeService = scanCodeService;
            _linksyConfig = linksyConfig;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<AddQrCodeToUmtParameterResponse> Handle(AddQrCodeToUmtParameter request, CancellationToken cancellationToken)
        {
            var umtParameter = await _umtParameterRepository.GetByIdAsync(request.UmtParameterId, cancellationToken, u => u.Url) ??
                throw new UmtParameterNotFoundException(request.UmtParameterId);
            var userId = _contextService.Identity!.Id;

            var (qrCodeImageUrlPath, fileName) = await _scanCodeService.GenerateQrCodeAsync(umtParameter.Url.Code, umtParameter.UmtSource, umtParameter.UmtMedium, umtParameter.UmtCampaign, 
                userId, cancellationToken);

            var qrCode = QrCode.CreateQrCode(new Image(qrCodeImageUrlPath, fileName), request.Tags, userId);
            umtParameter.AddQrCode(qrCode);

            await _umtParameterRepository.UpdateAsync(cancellationToken);

            _logger.LogInformation("QR code added to UMT parameter with ID: {UmtParameterId} by user with ID: {UserId}.", 
                request.UmtParameterId, userId);
            return new AddQrCodeToUmtParameterResponse(qrCode.Id, qrCodeImageUrlPath, fileName);
        }
    }
}
