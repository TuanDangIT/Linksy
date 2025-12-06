using Linksy.Application.Abstractions;
using Linksy.Application.Statistics.Analytics;
using Linksy.Application.Statistics.Features.GetLandingPageViewAnalytics;
using Linksy.Infrastructure.Exceptions;
using Linksy.Infrastructure.Statistics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Handlers
{
    internal class GetLandingPageViewAnalyticsHandler : IQueryHandler<GetLandingPageViewAnalytics, AnalyticsResponse>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly IAnalyticsService _analyticsService;
        private readonly IContextService _contextService;
        private readonly ILogger<GetLandingPageViewAnalyticsHandler> _logger;

        public GetLandingPageViewAnalyticsHandler(LinksyDbContext dbContext, IAnalyticsService analyticsService, IContextService contextService,
            ILogger<GetLandingPageViewAnalyticsHandler> logger)
        {
            _dbContext = dbContext;
            _analyticsService = analyticsService;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<AnalyticsResponse> Handle(GetLandingPageViewAnalytics request, CancellationToken cancellationToken)
        {
            var doesLandingPageExist = await _dbContext.LandingPages.AsNoTracking().AnyAsync(lp => lp.Id == request.LandingPageId);
            if (doesLandingPageExist is false)
            {
                throw new LandingPageNotFoundException(request.LandingPageId);
            }
            var query = _dbContext.LandingPageViews
                .Where(lp => lp.LandingPageId == request.LandingPageId)
                .AsNoTracking()
                .AsQueryable();
            var views = await _analyticsService.GetViewAnalyticsAsync(query, request, cancellationToken);
            _logger.LogInformation("Retrieved landing page view analytics for LandingPageId: {LandingPageId} by UserId: {UserId}.", request.LandingPageId, _contextService.Identity!.Id);
            return views;
        }
    }
}
