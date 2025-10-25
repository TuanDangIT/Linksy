using Linksy.Application.Abstractions;
using Linksy.Application.QrCodes.Features.CreateQrCode;
using Linksy.Application.Shared.BlobStorage;
using Linksy.Application.Shared.Configuration;
using Linksy.Application.Shared.ScanCodes;
using Linksy.Domain.DomainServices;
using Linksy.Domain.Entities;
using Linksy.Domain.Repositories;
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
        private readonly IBlobStorageService _blobStorageService;
        private readonly ILogger<CreateQrCodeHandler> _logger;

        public CreateBarcodeHandler(IBarcodeRepository barcodeRepository, IGenerateShotenedUrlService generateShotenedUrlService, IContextService contextService, 
            LinksyConfig linksyConfig, IScanCodeService scanCodeService, IBlobStorageService blobStorageService, ILogger<CreateQrCodeHandler> logger)
        {
            _barcodeRepository = barcodeRepository;
            _generateShotenedUrlService = generateShotenedUrlService;
            _contextService = contextService;
            _linksyConfig = linksyConfig;
            _scanCodeService = scanCodeService;
            _blobStorageService = blobStorageService;
            _logger = logger;
        }
        public async Task<CreateBarcodeResponse> Handle(CreateBarcode request, CancellationToken cancellationToken)
        {
            var umtParameters = request.Url.UmtParameters?.Select(u => UmtParameter.CreateUmtParameter(u.UmtSource, u.UmtMedium, u.UmtCampaign));
            var userId = _contextService.Identity!.Id;
            var url = await _generateShotenedUrlService.GenerateShortenedUrl(request.Url.OriginalUrl, request.Url.CustomCode, umtParameters, userId, cancellationToken);
            var barcode = Barcode.CreateBarcode(url, string.Empty, request.Tags, userId);
            await _barcodeRepository.CreateAsync(barcode, cancellationToken);
            var linksyUrl = _linksyConfig.BaseUrl + "/" + url.Code;
            var (barcodeUrlPath, fileName) = await _scanCodeService.GenerateBarcodeAsync(barcode, linksyUrl, cancellationToken);
            barcode.SetImageUrlPath(barcodeUrlPath);
            await _barcodeRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Barcode was created with ID: {BarcodeId} for URL ID: {UrlId} by user with ID: {UserId}.", barcode.Id, url.Id, userId);
            return new CreateBarcodeResponse(barcode.Id, url.Id, barcodeUrlPath, fileName);
        }
    }
}
