using Linksy.Application.Abstractions;
using Linksy.Application.QrCodes.Features.CreateQrCode;
using Linksy.Application.Shared.BlobStorage;
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

namespace Linksy.Application.Barcodes.Features.CreateBarcode
{
    internal class CreateBarcodeHandler : ICommandHandler<CreateBarcode, CreateBarcodeResponse>
    {
        private readonly IBarcodeRepository _barcodeRepository;
        private readonly IGenerateShotenedUrlService _generateShotenedUrlService;
        private readonly IContextService _contextService;
        private readonly LinksyConfig _linksyConfig;
        private readonly IScanCodeService _scanCodeService;
        private readonly ILogger<CreateQrCodeHandler> _logger;

        public CreateBarcodeHandler(IBarcodeRepository barcodeRepository, IGenerateShotenedUrlService generateShotenedUrlService, IContextService contextService, 
            LinksyConfig linksyConfig, IScanCodeService scanCodeService, ILogger<CreateQrCodeHandler> logger)
        {
            _barcodeRepository = barcodeRepository;
            _generateShotenedUrlService = generateShotenedUrlService;
            _contextService = contextService;
            _linksyConfig = linksyConfig;
            _scanCodeService = scanCodeService;
            _logger = logger;
        }
        public async Task<CreateBarcodeResponse> Handle(CreateBarcode request, CancellationToken cancellationToken)
        {
            var userId = _contextService.Identity!.Id;
            var umtParameters = request.Url.UmtParameters?.Select(u => UmtParameter.CreateUmtParameter(u.UmtSource, u.UmtMedium, u.UmtCampaign, userId));
            var url = await _generateShotenedUrlService.GenerateShortenedUrlAsync(request.Url.OriginalUrl, request.Url.CustomCode, request.Url.Tags, umtParameters, userId, cancellationToken);
            var barcodeQueryParameter = _linksyConfig.ScanCode.BarcodeQueryParameter + "=true";
            var linksyUrl = _linksyConfig.BaseUrl + "/" + url.Code + "?" + barcodeQueryParameter;
            var (barcodeUrlPath, fileName) = await _scanCodeService.GenerateBarcodeAsync(linksyUrl, userId, cancellationToken);
            var barcode = Barcode.CreateBarcode(url, new Image(barcodeUrlPath, fileName), request.Tags, userId);
            await _barcodeRepository.CreateAsync(barcode, cancellationToken);
            _logger.LogInformation("Barcode was created with ID: {BarcodeId} for URL ID: {UrlId} by user with ID: {UserId}.", barcode.Id, url.Id, userId);
            return new CreateBarcodeResponse(barcode.Id, url.Id, barcodeUrlPath, fileName);
        }
    }
}
