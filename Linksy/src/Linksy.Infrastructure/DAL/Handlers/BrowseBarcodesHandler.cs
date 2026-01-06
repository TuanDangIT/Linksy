using Linksy.Application.Abstractions;
using Linksy.Application.Barcodes.Features.BrowseBarcodes;
using Linksy.Application.QrCodes.Features.BrowseQrCodes;
using Linksy.Application.Shared.DTO;
using Linksy.Domain.Entities.ScanCode;
using Linksy.Infrastructure.Pagination.Configuration;
using Linksy.Infrastructure.Pagination.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Handlers
{
    internal class BrowseBarcodesHandler : IQueryHandler<BrowseBarcodes, BrowseBarcodesResponse>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly IPaginationService<Barcode> _paginationService;
        private readonly IContextService _contextService;
        private readonly ILogger<BrowseBarcodesHandler> _logger;

        public BrowseBarcodesHandler(LinksyDbContext dbContext, IPaginationService<Barcode> paginationService,
            IContextService contextService, ILogger<BrowseBarcodesHandler> logger)
        {
            _dbContext = dbContext;
            _paginationService = paginationService;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<BrowseBarcodesResponse> Handle(BrowseBarcodes request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Barcodes
                .AsNoTracking()
                .AsQueryable();

            var orders = request.Sort?.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            var result = await _paginationService.PaginateAsync(query, request.PageNumber, request.PageSize, request.Filters, orders,
                q => new BrowseBarcodeDto(q.Id, new BrowseScanCodesUrlDto(q.Url.Id, q.Url.OriginalUrl, q.Url.Code), q.Url.TagsList, q.ScanCount, q.CreatedAt, q.UpdatedAt), cancellationToken);
            _logger.LogInformation("Browsed barcodes by user with ID {UserId}.", _contextService.Identity!.Id);
            return new BrowseBarcodesResponse(result);
        }
    }
}
