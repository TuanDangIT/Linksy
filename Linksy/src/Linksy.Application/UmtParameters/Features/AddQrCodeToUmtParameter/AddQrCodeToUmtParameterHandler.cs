using Linksy.Application.Abstractions;
using Linksy.Application.Shared.BlobStorage;
using Linksy.Application.Shared.Configuration;
using Linksy.Application.Shared.ScanCodes;
using Linksy.Application.UmtParameters.Exceptions;
using Linksy.Application.Urls.Exceptions;
using Linksy.Domain.Entities.ScanCode;
using Linksy.Domain.Repositories;
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
        private readonly IUrlRepository _urlRepository;
        private readonly IScanCodeService _scanCodeService;
        private readonly LinksyConfig _linksyConfig;
        private readonly IContextService _contextService;
        private readonly ILogger<AddQrCodeToUmtParameterHandler> _logger;

        public AddQrCodeToUmtParameterHandler(IUrlRepository urlRepository, IScanCodeService scanCodeService, 
            LinksyConfig linksyConfig, IContextService contextService, ILogger<AddQrCodeToUmtParameterHandler> logger)
        {
            _urlRepository = urlRepository;
            _scanCodeService = scanCodeService;
            _linksyConfig = linksyConfig;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<AddQrCodeToUmtParameterResponse> Handle(AddQrCodeToUmtParameter request, CancellationToken cancellationToken)
        {
            var url = await _urlRepository.GetUrlAsync(request.UrlId, cancellationToken, u => u.UmtParameters) ?? throw new UrlNotFoundException(request.UrlId);
            var umtParameter = url.UmtParameters.SingleOrDefault(u => u.Id == request.UmtParameterId) ?? throw new UmtParameterNotFoundException(request.UmtParameterId);
            var userId = _contextService.Identity!.Id;
            var qrCode = QrCode.CreateQrCode(string.Empty, request.Tags, userId);
            umtParameter.AddQrCode(qrCode);
            await _urlRepository.UpdateAsync(cancellationToken);

            var umtSourceQueryParameter = umtParameter.UmtSource is not null ? $"umt_source={umtParameter.UmtSource}&" : string.Empty;
            var umtMediumQueryParameter = umtParameter.UmtMedium is not null ? $"&umt_medium={umtParameter.UmtMedium}&" : string.Empty;
            var umtCampainQueryParameter = umtParameter.UmtCampaign is not null ? $"&umt_campaign={umtParameter.UmtCampaign}&" : string.Empty;
            var qrCodeQueryParameter = umtSourceQueryParameter + umtMediumQueryParameter + umtCampainQueryParameter + _linksyConfig.ScanCode.QrCodeQueryParameter + "=true";
            var linksyUrl = _linksyConfig.BaseUrl + "/" + url.Code + "?" + qrCodeQueryParameter;
            var (qrCodeImageUrlPath, fileName) = await _scanCodeService.GenerateQrCodeAsync(qrCode, linksyUrl, cancellationToken);

            qrCode.SetImageUrlPath(qrCodeImageUrlPath);
            await _urlRepository.UpdateAsync(cancellationToken);

            _logger.LogInformation("QR code added to UMT parameter with ID: {UmtParameterId} of URL with ID: {UrlId} by user with ID: {UserId}.", 
                request.UmtParameterId, request.UrlId, userId);
            return new AddQrCodeToUmtParameterResponse(qrCode.Id, qrCodeImageUrlPath, fileName);
        }
    }
}
