using Linksy.Application.Abstractions;
using Linksy.Application.Statistics.Analytics;
using Linksy.Application.Statistics.Features.GetLandingPageEngagementAnalytics;
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
    internal class GetLandingPageEngagementAnalyticsHandler : IQueryHandler<GetLandingPageEngagementAnalytics, AnalyticsResponse>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly IAnalyticsService _analyticsService;
        private readonly IContextService _contextService;
        private readonly ILogger<GetLandingPageEngagementAnalyticsHandler> _logger;

        public GetLandingPageEngagementAnalyticsHandler(LinksyDbContext dbContext, IAnalyticsService analyticsService, IContextService contextService, 
            ILogger<GetLandingPageEngagementAnalyticsHandler> logger)
        {
            _dbContext = dbContext;
            _analyticsService = analyticsService;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<AnalyticsResponse> Handle(GetLandingPageEngagementAnalytics request, CancellationToken cancellationToken)
        {
            var doesLandingPageExist = await _dbContext.LandingPages.AsNoTracking().AnyAsync(lp => lp.Id == request.LandingPageId, cancellationToken);
            if (doesLandingPageExist is false)
            {
                throw new LandingPageNotFoundException(request.LandingPageId);
            }
            var query = _dbContext.LandingPageEngagements
                .Where(lp => lp.LandingPageId == request.LandingPageId)
                .AsNoTracking()
                .AsQueryable();
            var analytics = await _analyticsService.GetEngagementAnalyticsAsync(query, request, cancellationToken);
            _logger.LogInformation("Retrieved landing page engagement analytics for LandingPageId: {LandingPageId} by UserId: {UserId}.", request.LandingPageId, _contextService.Identity!.Id);
            return analytics;
        }
    }
}
