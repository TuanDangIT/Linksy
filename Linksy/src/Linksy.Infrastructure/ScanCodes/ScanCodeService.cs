using BarcodeStandard;
using Linksy.Application.Shared.BlobStorage;
using Linksy.Application.Shared.Configuration;
using Linksy.Application.Shared.ScanCodes;
using Linksy.Domain.Entities.ScanCode;
using Linksy.Domain.Entities.Tracking;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.ScanCodes
{
    internal class ScanCodeService : IScanCodeService
    {
        private readonly IBlobStorageService _blobStorageService;
        private readonly LinksyConfig _linksyConfig;
        private readonly TimeProvider _timeProvider;
        private readonly ILogger<ScanCodeService> _logger;
        private readonly string _contentType = "image/png";
        private readonly string _pngExtension = ".png";

        public ScanCodeService(IBlobStorageService blobStorageService, LinksyConfig linksyConfig, TimeProvider timeProvider, ILogger<ScanCodeService> logger)
        {
            _blobStorageService = blobStorageService;
            _linksyConfig = linksyConfig;
            _timeProvider = timeProvider;
            _logger = logger;
        }

        public async Task<(string QrCodeUrlPath, string FileName)> GenerateQrCodeAsync(string url, int userId, CancellationToken cancellationToken = default)
        {
            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            using var generatedQrCode = new PngByteQRCode(qrCodeData);
            var stream = new MemoryStream(generatedQrCode.GetGraphic(20));
            var timestamp = _timeProvider.GetUtcNow().ToString("yyyyMMddTHHmmssZ");
            var entityName = nameof(QrCode).ToLower();
            var fileName = _linksyConfig.BlobStorage.QrCodesPrefixPathFromContainer + $"{entityName}-{timestamp}{_pngExtension}";
            var file = new FormFile(stream, 0, stream.Length, entityName, fileName)
            {
                Headers = new HeaderDictionary()
            };
            file.ContentType = _contentType;
            var containerName = $"user-{userId}";
            var qrCodeUrlPath = await _blobStorageService.UploadAsync(file, file.FileName, containerName, cancellationToken);
            _logger.LogDebug("Generated QR code for URL: {url}", url);
            return (qrCodeUrlPath, file.FileName);
        }

        public async Task<(string BarcodeUrlPath, string FileName)> GenerateBarcodeAsync(string url, int userId, CancellationToken cancellationToken = default)
        {
            var barcodeGenerator = new BarcodeStandard.Barcode();
            barcodeGenerator.Encode(BarcodeStandard.Type.Code128, url, 800, 150);
            var barcodeBytes = barcodeGenerator.GetImageData(SaveTypes.Png);
            var stream = new MemoryStream(barcodeBytes);
            var timestamp = _timeProvider.GetUtcNow().ToString("yyyyMMddTHHmmssZ");
            var entityName = nameof(Domain.Entities.ScanCode.Barcode).ToLower();
            var fileName = _linksyConfig.BlobStorage.BarcodesPrefixPathFromContainer + $"{entityName}-{timestamp}{_pngExtension}";
            var file = new FormFile(stream, 0, stream.Length, entityName, fileName)
            {
                Headers = new HeaderDictionary()
            };
            file.ContentType = _contentType;
            var containerName = $"user-{userId}";
            var barcodeUrlPath = await _blobStorageService.UploadAsync(file, file.FileName, containerName, cancellationToken);
            _logger.LogDebug("Generated barcode for URL: {url}", url);  
            return (barcodeUrlPath, file.FileName);
        }

        public async Task DeleteAsync(string fileName, int userId, CancellationToken cancellationToken = default)
        {
            var containerName = $"user-{userId}";
            await _blobStorageService.DeleteAsync(fileName, containerName, cancellationToken);
            _logger.LogDebug(fileName + " deleted from container " + containerName);
        }
    }
}
