using Linksy.Application.Abstractions;
using Linksy.Application.Shared.Configuration;
using Linksy.Application.Shared.ScanCodes;
using Linksy.Domain.Entities.ScanCode;
using Linksy.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.DeleteUrl
{
    internal class DeleteUrlHandler : ICommandHandler<DeleteUrl>
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IContextService _contextService;
        private readonly LinksyConfig _linksyConfig;
        private readonly IScanCodeService _scanCodeService;
        private readonly ILogger<DeleteUrlHandler> _logger;

        public DeleteUrlHandler(IUrlRepository urlRepository, IContextService contextService, LinksyConfig linksyConfig, IScanCodeService scanCodeService, 
            ILogger<DeleteUrlHandler> logger)
        {
            _urlRepository = urlRepository;
            _contextService = contextService;
            _linksyConfig = linksyConfig;
            _scanCodeService = scanCodeService;
            _logger = logger;
        }
        public async Task Handle(DeleteUrl request, CancellationToken cancellationToken)
        {
            var url = await _urlRepository.GetUrlAsync(request.Id, cancellationToken, u => u.Barcode, u => u.QrCode);
            var userId = _contextService.Identity!.Id;
            if (url is null)
            {
                return;
            }
            if(url.QrCode is not null)
            {
                var fileName = url.QrCode.ScanCodeImage.FileName;
                await _scanCodeService.DeleteAsync(fileName, userId);
                _logger.LogInformation("QR Code image file {FileName} and ID {QrCodeId} deleted from blob storage by user with ID: {userId}.", fileName, url.QrCode.Id, userId);
            }
            if(url.Barcode is not null)
            {
                var fileName = url.Barcode.ScanCodeImage.FileName;
                await _scanCodeService.DeleteAsync(fileName, userId);
                _logger.LogInformation("Barcode image file {FileName} and ID {BarcodeId} deleted from blob storage by user with ID: {userId}.", fileName, url.Barcode.Id, userId);
            }
            await _urlRepository.DeleteAsync(url.Id, cancellationToken);
            _logger.LogInformation("Url with id {Id} was deleted by user with ID: {userId}.", request.Id, userId);
        }
    }
}
