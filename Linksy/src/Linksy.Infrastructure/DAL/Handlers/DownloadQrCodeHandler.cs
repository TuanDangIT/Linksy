using Linksy.Application.Abstractions;
using Linksy.Application.Barcodes.Exceptions;
using Linksy.Application.Barcodes.Features.DownloadBarcode;
using Linksy.Application.QrCodes.Exceptions;
using Linksy.Application.QrCodes.Features.DownloadQrCode;
using Linksy.Application.Shared.BlobStorage;
using Linksy.Application.Shared.Configuration;
using Linksy.Domain.Entities.ScanCode;
using Linksy.Infrastructure.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Handlers
{
    internal class DownloadQrCodeHandler : IQueryHandler<DownloadQrCode, FileStreamResult>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IContextService _contextService;
        private readonly ILogger<DownloadQrCodeHandler> _logger;
        private const string _containerName = "qrcodes";
        private const string _pngExtension = ".png";

        public DownloadQrCodeHandler(LinksyDbContext dbContext, IBlobStorageService blobStorageService, IContextService contextService, ILogger<DownloadQrCodeHandler> logger)
        {
            _dbContext = dbContext;
            _blobStorageService = blobStorageService;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<FileStreamResult> Handle(DownloadQrCode request, CancellationToken cancellationToken)
        {
            var qrCode = await _dbContext.QrCodes
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == request.QrCodeId, cancellationToken) ??
                throw new QrCodeNotFoundException(request.QrCodeId);
            var imageUrlPath = qrCode.ImageUrlPath;
            var dto = await _blobStorageService.DownloadAsync($"{nameof(QrCode)}-{qrCode.Id}{_pngExtension}", _containerName, cancellationToken);
            var fileResult = new FileStreamResult(dto.FileStream, dto.ContentType)
            {
                FileDownloadName = $"{nameof(QrCode)}-{qrCode.Id}{_pngExtension}"
            };
            _logger.LogInformation("QR Code with ID: {QrCodeId} downloaded successfully by user with ID: {UserId}.", request.QrCodeId, _contextService.Identity!.Id);
            return fileResult;
        }
    }
}
