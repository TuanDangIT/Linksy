using Linksy.Application.Abstractions;
using Linksy.Application.Urls.Features.BrowseUrls;
using Linksy.Domain.Entities.Url;
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
    internal class BrowseUrlsHandler : IQueryHandler<BrowseUrls, BrowseUrlsResponse>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly IPaginationService<Url> _paginationService;
        private readonly IContextService _contextService;
        private readonly ILogger<BrowseQrCodesHandler> _logger;

        public BrowseUrlsHandler(LinksyDbContext dbContext, IPaginationService<Url> paginationService,
            IContextService contextService, ILogger<BrowseQrCodesHandler> logger)
        {
            _dbContext = dbContext;
            _paginationService = paginationService;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<BrowseUrlsResponse> Handle(BrowseUrls request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Urls
                .AsNoTracking()
                .AsQueryable();

            var result = await _paginationService.PaginateAsync(query, request.PageNumber, request.PageSize, request.Filters, request.Orders,
                u => new BrowseUrlDto(u.Id, u.OriginalUrl, u.Code, u.VisitCount, u.IsActive, u.QrCode != null, u.Barcode != null, 
                u.ImageLandingPageItems.Any() || u.UrlLandingPageItems.Any(), u.UmtParameters.Any(), u.TagsList, u.CreatedAt, u.UpdatedAt), cancellationToken);
            _logger.LogInformation("User with ID: {userId} browsed URLs.", _contextService.Identity?.Id);
            return new BrowseUrlsResponse(result);
        }
    }
}
