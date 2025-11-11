using Linksy.Application.Abstractions;
using Linksy.Application.Barcodes.Exceptions;
using Linksy.Application.Barcodes.Features.DownloadBarcode;
using Linksy.Application.Shared.BlobStorage;
using Linksy.Application.Shared.Configuration;
using Linksy.Domain.Entities.ScanCode;
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
    internal class DownloadBarcodeHandler : IQueryHandler<DownloadBarcode, FileStreamResult>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IContextService _contextService;
        private readonly ILogger<DownloadBarcodeHandler> _logger;
        private const string _containerName = "barcodes";
        private const string _pngExtension = ".png";

        public DownloadBarcodeHandler(LinksyDbContext dbContext, IBlobStorageService blobStorageService, IContextService contextService, ILogger<DownloadBarcodeHandler> logger)
        {
            _dbContext = dbContext;
            _blobStorageService = blobStorageService;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<FileStreamResult> Handle(DownloadBarcode request, CancellationToken cancellationToken)
        {
            var barcode = await _dbContext.Barcodes
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == request.BarcodeId, cancellationToken) ?? 
                throw new BarcodeNotFoundException(request.BarcodeId);
            var imageUrlPath = barcode.ImageUrlPath;
            var dto = await _blobStorageService.DownloadAsync($"{nameof(Barcode)}-{barcode.Id}{_pngExtension}", _containerName, cancellationToken);
            var fileResult = new FileStreamResult(dto.FileStream, dto.ContentType)
            {
                FileDownloadName = $"{nameof(Barcode)}-{barcode.Id}{_pngExtension}"
            };  
            _logger.LogInformation("Barcode with ID: {BarcodeId} downloaded successfully by user with ID: {UserId}.", request.BarcodeId, _contextService.Identity!.Id);
            return fileResult;
        }
    }
}
