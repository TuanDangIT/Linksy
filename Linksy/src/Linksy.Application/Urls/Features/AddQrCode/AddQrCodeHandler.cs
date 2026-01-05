using Linksy.Application.Abstractions;
using Linksy.Application.Shared.Configuration;
using Linksy.Application.Shared.ScanCodes;
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

namespace Linksy.Application.Urls.Features.AddQrCode
{
    internal class AddQrCodeHandler : ICommandHandler<AddQrCode, AddQrCodeResponse>
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IScanCodeService _scanCodeService;
        private readonly IContextService _contextService;
        private readonly LinksyConfig _linksyConfig;
        private readonly ILogger<AddQrCodeHandler> _logger;

        public AddQrCodeHandler(IUrlRepository urlRepository, IScanCodeService scanCodeService, IContextService contextService, LinksyConfig linksyConfig,
            ILogger<AddQrCodeHandler> logger)
        {
            _urlRepository = urlRepository;
            _scanCodeService = scanCodeService;
            _contextService = contextService;
            _linksyConfig = linksyConfig;
            _logger = logger;
        }
        public async Task<AddQrCodeResponse> Handle(AddQrCode request, CancellationToken cancellationToken)
        {
            var url = await _urlRepository.GetUrlAsync(request.UrlId, cancellationToken, u => u.QrCode) ?? throw new UrlNotFoundException(request.UrlId);
            if(url.QrCode is not null)
            {
                throw new UrlQrCodeAlreadyExistsException(request.UrlId);
            }
            var userId = _contextService.Identity!.Id;
            var (qrCodeUrlPath, fileName) = await _scanCodeService.GenerateQrCodeAsync(url.Code, userId, cancellationToken);
            var qrCode = QrCode.CreateQrCode(url, new Image(qrCodeUrlPath, fileName), request.Tags, userId);
            url.AddQrCode(qrCode);
            await _urlRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("QR code added to URL with ID: {UrlId} by user with ID: {UserId}.", request.UrlId, userId);
            return new AddQrCodeResponse(qrCode.Id, qrCodeUrlPath, fileName);
        }
    }
}
