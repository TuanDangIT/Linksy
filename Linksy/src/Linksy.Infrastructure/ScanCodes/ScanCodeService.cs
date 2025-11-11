using BarcodeStandard;
using Linksy.Application.Shared.BlobStorage;
using Linksy.Application.Shared.Configuration;
using Linksy.Application.Shared.ScanCodes;
using Linksy.Domain.Entities.ScanCode;
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
        private readonly ILogger<ScanCodeService> _logger;
        private readonly string _contentType = "image/png";
        private readonly string _pngExtension = ".png";

        public ScanCodeService(IBlobStorageService blobStorageService, LinksyConfig linksyConfig, ILogger<ScanCodeService> logger)
        {
            _blobStorageService = blobStorageService;
            _linksyConfig = linksyConfig;
            _logger = logger;
        }

        public async Task<(string QrCodeUrlPath, string FileName)> GenerateQrCodeAsync(QrCode qrCode, string url, CancellationToken cancellationToken = default)
        {
            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            using var generatedQrCode = new PngByteQRCode(qrCodeData);
            var stream = new MemoryStream(generatedQrCode.GetGraphic(20));
            var file = new FormFile(stream, 0, stream.Length, nameof(QrCode), $"{nameof(QrCode)}-{qrCode.Id}{_pngExtension}")
            {
                Headers = new HeaderDictionary()
            };
            file.ContentType = _contentType;
            var qrCodeUrlPath = await _blobStorageService.UploadAsync(file, file.FileName, _linksyConfig.BlobStorage.QrCodesContainerName, cancellationToken);
            _logger.LogDebug("Generated QR code for URL: {url}", url);
            return (qrCodeUrlPath, file.FileName);
        }

        public async Task<(string BarcodeUrlPath, string FileName)> GenerateBarcodeAsync(Domain.Entities.ScanCode.Barcode barcode, string url, CancellationToken cancellationToken = default)
        {
            var barcodeGenerator = new BarcodeStandard.Barcode();
            barcodeGenerator.Encode(BarcodeStandard.Type.Code128, url, 800, 150);
            var barcodeBytes = barcodeGenerator.GetImageData(SaveTypes.Png);
            var stream = new MemoryStream(barcodeBytes);
            var file = new FormFile(stream, 0, stream.Length, nameof(Domain.Entities.ScanCode.Barcode), $"{nameof(Domain.Entities.ScanCode.Barcode)}-{barcode.Id}{_pngExtension}")
            {
                Headers = new HeaderDictionary()
            };
            file.ContentType = _contentType;
            var barcodeUrlPath = await _blobStorageService.UploadAsync(file, file.FileName, _linksyConfig.BlobStorage.BarcodesContainerName, cancellationToken);
            _logger.LogDebug("Generated barcode for URL: {url}", url);  
            return (barcodeUrlPath, file.FileName);
        }

        public async Task DeleteAsync(string fileName, string containerName, CancellationToken cancellationToken = default)
        {
            await _blobStorageService.DeleteAsync(fileName, containerName, cancellationToken);
            _logger.LogDebug(fileName + " deleted from container " + containerName);
        }

        public string GetScanCodeFileName(int scanCodeId, string scanCodeType)
            => $"{scanCodeType}-{scanCodeId}{_pngExtension}";
    }
}
