using BarcodeStandard;
using Linksy.Application.Shared.BlobStorage;
using Linksy.Application.Shared.Configuration;
using Linksy.Application.Shared.ScanCodes;
using Linksy.Domain.Entities.ScanCode;
using Linksy.Domain.Entities.Tracking;
using Linksy.Domain.Entities.Url;
using Linksy.Domain.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
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
        private readonly string _qrCodeQueryParameter = "IsQrCode=true";
        private readonly string _barcodeQueryParameter = "IsBarcode=true";
        private readonly int _pixelsPerModule = 20;

        public ScanCodeService(IBlobStorageService blobStorageService, LinksyConfig linksyConfig, TimeProvider timeProvider, ILogger<ScanCodeService> logger)
        {
            _blobStorageService = blobStorageService;
            _linksyConfig = linksyConfig;
            _timeProvider = timeProvider;
            _logger = logger;
        }

        public async Task<(string QrCodeUrlPath, string FileName)> GenerateQrCodeAsync(string code, int userId, CancellationToken cancellationToken = default)
        {
            var url = _linksyConfig.BaseUrl + "/" + code + "?" + _qrCodeQueryParameter;
            var stream = GenerateQrCode(url);
            var entityName = nameof(QrCode).ToLower();
            var fileName = CreateFileName(_linksyConfig.BlobStorage.QrCodesPrefixPathFromContainer, entityName);
            var fileData = await UploadScanCodeToStorageAsync(fileName, entityName, stream, userId, cancellationToken);
            _logger.LogDebug("Generated QR code for URL: {url}", url);

            return (fileData.UrlPath, fileData.FileName);
        }

        public async Task<(string QrCodeUrlPath, string FileName)> GenerateQrCodeAsync(string code, string? umtSource, string? umtMedium, string? umtCampaign, int userId, CancellationToken cancellationToken = default)
        {
            var umtQuery = BuildUtmQuery(umtSource, umtMedium, umtCampaign);
            var qrCodeQueryParameter = umtQuery + _qrCodeQueryParameter;
            var url = _linksyConfig.BaseUrl + "/" + code + "?" + qrCodeQueryParameter;
            var stream = GenerateQrCode(url);
            var entityName = nameof(QrCode).ToLower();
            var fileName = CreateFileName(_linksyConfig.BlobStorage.QrCodesPrefixPathFromContainer, entityName);
            var fileData = await UploadScanCodeToStorageAsync(fileName, entityName, stream, userId, cancellationToken);
            _logger.LogDebug("Generated QR code for URL: {url}", url);

            return (fileData.UrlPath, fileData.FileName);
        }

        public async Task<(string BarcodeUrlPath, string FileName)> GenerateBarcodeAsync(string code, int userId, CancellationToken cancellationToken = default)
        {
            var url = _linksyConfig.BaseUrl + "/" + code + "?" + _barcodeQueryParameter;
            var barcodeGenerator = new BarcodeStandard.Barcode();
            barcodeGenerator.Encode(BarcodeStandard.Type.Code128, url, 800, 150);
            var barcodeBytes = barcodeGenerator.GetImageData(SaveTypes.Png);
            var stream = new MemoryStream(barcodeBytes);
            var entityName = nameof(Domain.Entities.ScanCode.Barcode).ToLower();
            var fileName = CreateFileName(_linksyConfig.BlobStorage.BarcodesPrefixPathFromContainer, entityName);
            var fileData = await UploadScanCodeToStorageAsync(fileName, entityName, stream, userId, cancellationToken);
            _logger.LogDebug("Generated barcode for URL: {url}", url);
            return (fileData.UrlPath, fileData.FileName);
        }

        public async Task DeleteAsync(string fileName, int userId, CancellationToken cancellationToken = default)
        {
            var containerName = $"user-{userId}";
            await _blobStorageService.DeleteAsync(fileName, containerName, cancellationToken);
            _logger.LogDebug(fileName + " deleted from container " + containerName);
        }

        private Stream GenerateQrCode(string url)
        {
            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            using var generatedQrCode = new PngByteQRCode(qrCodeData);
            var stream = new MemoryStream(generatedQrCode.GetGraphic(_pixelsPerModule));
            return stream;
        }

        private async Task<(string UrlPath, string FileName)> UploadScanCodeToStorageAsync(string fileName, string entityName, Stream stream, int userId, CancellationToken cancellationToken = default)
        {
            var file = new FormFile(stream, 0, stream.Length, entityName, fileName)
            {
                Headers = new HeaderDictionary()
            };
            file.ContentType = _contentType;
            var containerName = $"user-{userId}";
            var urlPath = await _blobStorageService.UploadAsync(file, file.FileName, containerName, cancellationToken);

            return (urlPath, file.FileName);
        }

        private string BuildUtmQuery(string? source, string? medium, string? campaign)
        {
            var parameters = new List<string>();

            if (!string.IsNullOrWhiteSpace(source))
                parameters.Add($"utm_source={source}");

            if (!string.IsNullOrWhiteSpace(medium))
                parameters.Add($"utm_medium={medium}");

            if (!string.IsNullOrWhiteSpace(campaign))
                parameters.Add($"utm_campaign={campaign}");

            return parameters.Count > 0
                ? string.Join("&", parameters) + "&"
                : string.Empty;
        }

        private string CreateFileName(string prefix, string entityName)
        {
            var timestamp = _timeProvider.GetUtcNow().ToString("yyyyMMddTHHmmssZ");
            return $"{prefix}{entityName}-{timestamp}{_pngExtension}";
        }

    }
}
