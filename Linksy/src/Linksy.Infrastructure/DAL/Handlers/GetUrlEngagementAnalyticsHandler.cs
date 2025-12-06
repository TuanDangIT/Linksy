using Linksy.Application.Abstractions;
using Linksy.Application.Statistics.Analytics;
using Linksy.Application.Statistics.Features.GetUrlEngagementAnalytics;
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
    internal class GetUrlEngagementAnalyticsHandler : IQueryHandler<GetUrlEngagementAnalytics, AnalyticsResponse>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly IAnalyticsService _analyticsService;
        private readonly IContextService _contextService;
        private readonly ILogger<GetUrlEngagementAnalyticsHandler> _logger;

        public GetUrlEngagementAnalyticsHandler(LinksyDbContext dbContext, IAnalyticsService analyticsService, IContextService contextService, ILogger<GetUrlEngagementAnalyticsHandler> logger)
        {
            _dbContext = dbContext;
            _analyticsService = analyticsService;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<AnalyticsResponse> Handle(GetUrlEngagementAnalytics request, CancellationToken cancellationToken)
        {
            var doesUrlExist = await _dbContext.Urls.AsNoTracking().AnyAsync(u => u.Id == request.UrlId, cancellationToken);
            if (doesUrlExist is false)
            {
                throw new UrlNotFoundException(request.UrlId);
            }
            var query = _dbContext.UrlEngagements
                .Where(u => u.UrlId == request.UrlId)
                .AsNoTracking()
                .AsQueryable();
            var analytics = await _analyticsService.GetEngagementAnalyticsAsync(query, request, cancellationToken);
            _logger.LogInformation("Retrieved URL engagement analytics for UrlId: {UrlId} by UserId: {UserId}.", request.UrlId, _contextService.Identity!.Id);
            return analytics;
        }
    }
}
