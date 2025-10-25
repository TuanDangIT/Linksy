using Linksy.Application.Abstractions;
using Linksy.Application.Shared.Configuration;
using Linksy.Application.Shared.ScanCodes;
using Linksy.Application.Urls.Exceptions;
using Linksy.Domain.Entities;
using Linksy.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.AddBarcode
{
    internal class AddBarcodeHandler : ICommandHandler<AddBarcode, AddBarcodeResponse>
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IScanCodeService _scanCodeService;
        private readonly IContextService _contextService;
        private readonly LinksyConfig _linksyConfig;
        private readonly ILogger<AddBarcodeHandler> _logger;

        public AddBarcodeHandler(IUrlRepository urlRepository, IScanCodeService scanCodeService, IContextService contextService, 
            LinksyConfig linksyConfig, ILogger<AddBarcodeHandler> logger)
        {
            _urlRepository = urlRepository;
            _scanCodeService = scanCodeService;
            _contextService = contextService;
            _linksyConfig = linksyConfig;
            _logger = logger;
        }
        public async Task<AddBarcodeResponse> Handle(AddBarcode request, CancellationToken cancellationToken)
        {
            var url = await _urlRepository.GetUrlAsync(request.UrlId, cancellationToken, u => u.Barcode) ?? throw new UrlNotFoundException(request.UrlId);
            if(url.Barcode is not null)
            {
                throw new UrlBarcodeAlreadyExistsException(request.UrlId);
            }
            var userId = _contextService.Identity!.Id;
            var barcode = Barcode.CreateBarcode(url, string.Empty, request.Tags, userId);
            url.AddBarcode(barcode);
            await _urlRepository.UpdateAsync(cancellationToken);
            var linksyUrl = _linksyConfig.BaseUrl + "/" + url.Code;
            var (barcodeUrlPath, fileName) = await _scanCodeService.GenerateBarcodeAsync(barcode, linksyUrl, cancellationToken);
            barcode.SetImageUrlPath(barcodeUrlPath);
            await _urlRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Barcode added to URL with ID: {UrlId} by user with ID: {UserId}.", request.UrlId, userId);
            return new AddBarcodeResponse(barcode.Id, barcodeUrlPath, fileName);
        }
    }
}
