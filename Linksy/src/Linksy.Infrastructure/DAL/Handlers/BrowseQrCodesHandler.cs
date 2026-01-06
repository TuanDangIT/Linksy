using Linksy.Application.Abstractions;
using Linksy.Application.QrCodes.Features.BrowseQrCodes;
using Linksy.Application.Shared.DTO;
using Linksy.Application.Shared.Pagination;
using Linksy.Domain.Entities.ScanCode;
using Linksy.Infrastructure.Pagination.Configuration;
using Linksy.Infrastructure.Pagination.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Linksy.Infrastructure.DAL.Handlers
{
    internal class BrowseQrCodesHandler : IQueryHandler<BrowseQrCodes, BrowseQrCodeResponse>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly IPaginationService<QrCode> _paginationService;
        private readonly IContextService _contextService;
        private readonly ILogger<BrowseQrCodesHandler> _logger;

        public BrowseQrCodesHandler(LinksyDbContext dbContext, IPaginationService<QrCode> paginationService, 
            IContextService contextService, ILogger<BrowseQrCodesHandler> logger)
        {
            _dbContext = dbContext;
            _paginationService = paginationService;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<BrowseQrCodeResponse> Handle(BrowseQrCodes request, CancellationToken cancellationToken)
        {  
            var query = _dbContext.QrCodes
                .AsNoTracking()
                .AsQueryable();

            var orders = request.Sort?.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            var result = await _paginationService.PaginateAsync(
                query,
                request.PageNumber,
                request.PageSize,
                request.Filters,
                orders,
                q => q.Url != null
                    ? new BrowseQrCodeDto(
                        q.Id,
                        new BrowseScanCodesUrlDto(q.Url.Id, q.Url.OriginalUrl, q.Url.Code),
                        q.Url.TagsList,
                        q.ScanCount,
                        q.CreatedAt,
                        q.UpdatedAt)
                    : new BrowseQrCodeDto(
                        q.Id,
                        new BrowseQrCodesUmtParameterDto(q.UmtParameter!.Id, q.UmtParameter.UmtSource, q.UmtParameter.UmtMedium, q.UmtParameter.UmtCampaign, 
                        new BrowseQrCodesUmtParameterUrlDto(q.UmtParameter.Url.Id, q.UmtParameter.Url.OriginalUrl, q.UmtParameter.Url.Code)),
                        q.TagsList, 
                        q.ScanCount,
                        q.CreatedAt,
                        q.UpdatedAt),
                cancellationToken);

            _logger.LogInformation("Browsed QR Codes by user with ID {UserId}.", _contextService.Identity!.Id);
            return new BrowseQrCodeResponse(result);
        }
    }
}
