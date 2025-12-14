using Linksy.Application.Abstractions;
using Linksy.Application.Barcodes.Features.GetBarcode;
using Linksy.Application.Shared.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Handlers
{
    internal class GetBarcodeHandler : IQueryHandler<GetBarcode, GetBarcodeResponse?>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly IContextService _contextService;
        private readonly ILogger<GetBarcodeHandler> _logger;

        public GetBarcodeHandler(LinksyDbContext dbContext, IContextService contextService, ILogger<GetBarcodeHandler> logger)
        {
            _dbContext = dbContext;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<GetBarcodeResponse?> Handle(GetBarcode request, CancellationToken cancellationToken)
        {
            var barcode = await _dbContext.Barcodes
                .AsNoTracking()
                .Where(b => b.Id == request.BarcodeId)
                .Select(b => new GetBarcodeResponse(b.Id, b.ScanCount, new ImageDto(b.ScanCodeImage.UrlPath, b.ScanCodeImage.FileName), b.TagsList, b.CreatedAt, b.UpdatedAt,
                    new GetBarcodeUrlDto(b.Url.Id, b.Url.OriginalUrl, b.Url.Code)))
                .FirstOrDefaultAsync(cancellationToken);

            _logger.LogInformation("Retrieved barcode with ID: {BarcodeId} by user with ID: {UserId}.", request.BarcodeId, _contextService.Identity!.Id);
            return barcode;
        }
    }
}
