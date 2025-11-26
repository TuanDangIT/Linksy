using BarcodeStandard;
using Linksy.Application.Abstractions;
using Linksy.Application.Shared.Configuration;
using Linksy.Application.Shared.ScanCodes;
using Linksy.Domain.DomainServices;
using Linksy.Domain.Entities.ScanCode;
using Linksy.Domain.Entities.Url;
using Linksy.Domain.Repositories;
using Linksy.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.QrCodes.Features.CreateQrCode
{
    internal class CreateQrCodeHandler : ICommandHandler<CreateQrCode, CreateQrCodeResponse>
    {
        private readonly IQrCodeRepository _qrCodeRepository;
        private readonly IGenerateShotenedUrlService _generateShotenedUrlService;
        private readonly IContextService _contextService;
        private readonly LinksyConfig _linksyConfig;
        private readonly IScanCodeService _scanCodeService;
        private readonly ILogger<CreateQrCodeHandler> _logger;

        public CreateQrCodeHandler(IQrCodeRepository qrCodeRepository, IGenerateShotenedUrlService generateShotenedUrlService, IContextService contextService, 
            LinksyConfig linksyConfig, IScanCodeService scanCodeService, ILogger<CreateQrCodeHandler> logger)
        {
            _qrCodeRepository = qrCodeRepository;
            _generateShotenedUrlService = generateShotenedUrlService;
            _contextService = contextService;
            _linksyConfig = linksyConfig;
            _scanCodeService = scanCodeService;
            _logger = logger;
        }
        public async Task<CreateQrCodeResponse> Handle(CreateQrCode request, CancellationToken cancellationToken)
        {
            var userId = _contextService.Identity!.Id;
            var umtParameters = request.Url.UmtParameters?.Select(u => UmtParameter.CreateUmtParameter(u.UmtSource, u.UmtMedium, u.UmtCampaign, userId));
            var url = await _generateShotenedUrlService.GenerateShortenedUrlAsync(request.Url.OriginalUrl, request.Url.CustomCode, request.Url.Tags, umtParameters, userId, cancellationToken);
            var qrCodeQueryParameter = _linksyConfig.ScanCode.QrCodeQueryParameter + "=true";
            var linksyUrl = _linksyConfig.BaseUrl + "/" + url.Code + "?" + qrCodeQueryParameter;
            var (qrCodeUrlPath, fileName) = await _scanCodeService.GenerateQrCodeAsync(linksyUrl, userId, cancellationToken);

            var qrCode = QrCode.CreateQrCode(url, new Image(qrCodeUrlPath, fileName), request.Tags, userId);
            await _qrCodeRepository.CreateAsync(qrCode, cancellationToken);

            _logger.LogInformation("QR Code was created with ID: {QrCodeId} for URL ID: {UrlId} by user with ID: {UserId}.", qrCode.Id, url.Id, userId);
            return new CreateQrCodeResponse(qrCode.Id, url.Id, qrCodeUrlPath, fileName);
        }
    }
}
