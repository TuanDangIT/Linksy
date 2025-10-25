using Linksy.Application.Abstractions;
using Linksy.Application.Urls.Features.BrowseUrls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Handlers
{
    internal class BrowseUrlsHandler : IQueryHandler<BrowseUrls, BrowseUrlResponse>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly IContextService _contextService;
        private readonly ILogger<BrowseUrlsHandler> _logger;

        public BrowseUrlsHandler(LinksyDbContext dbContext, IContextService contextService, ILogger<BrowseUrlsHandler> logger)
        {
            _dbContext = dbContext;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<BrowseUrlResponse> Handle(BrowseUrls request, CancellationToken cancellationToken)
        {
            var urls = await _dbContext.Urls
                .AsNoTracking()
                .Include(u => u.QrCode)
                .Include(u => u.Barcode)
                .Include(u => u.LandingPageItem)
                .Select(u => new BrowseUrlDto(u.Id, u.OriginalUrl, u.Code, u.VisitCount, u.IsActive, u.QrCode != null, 
                u.Barcode != null, u.LandingPageItem != null, u.CreatedAt, u.UpdatedAt))
                .ToListAsync(cancellationToken);
            _logger.LogInformation("User with ID: {userId} browsed URLs.", _contextService.Identity?.Id);
            return new BrowseUrlResponse(urls);
        }
    }
}
