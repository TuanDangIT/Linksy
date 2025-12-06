using Linksy.Application.Abstractions;
using Linksy.Application.Statistics.Analytics;
using Linksy.Application.Statistics.Features.GetUmtParameterEngagementAnalytics;
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
    internal class GetUmtParameterEngagementAnalyticsHandler : IQueryHandler<GetUmtParameterEngagementAnalytics, AnalyticsResponse>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly IAnalyticsService _analyticsService;
        private readonly IContextService _contextService;
        private readonly ILogger<GetUmtParameterEngagementAnalyticsHandler> _logger;

        public GetUmtParameterEngagementAnalyticsHandler(LinksyDbContext dbContext, IAnalyticsService analyticsService, IContextService contextService,
            ILogger<GetUmtParameterEngagementAnalyticsHandler> logger)
        {
            _dbContext = dbContext;
            _analyticsService = analyticsService;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<AnalyticsResponse> Handle(GetUmtParameterEngagementAnalytics request, CancellationToken cancellationToken)
        {
            var doesUmtParameterExist = await _dbContext.UmtParameters.AsNoTracking().AnyAsync(u => u.Id == request.UmtParameterId, cancellationToken);
            if (doesUmtParameterExist is false)
            {
                throw new UmtParameterNotFoundException(request.UmtParameterId);
            }
            var query = _dbContext.UmtParameterEngagements
                .Where(u => u.UmtParameterId == request.UmtParameterId)
                .AsNoTracking()
                .AsQueryable();
            var analytics = await _analyticsService.GetEngagementAnalyticsAsync(query, request, cancellationToken);
            _logger.LogInformation("Retrieved UTM parameter engagement analytics for UmtParameterId: {UmtParameterId} by UserId: {UserId}.", request.UmtParameterId, _contextService.Identity!.Id);
            return analytics;
        }
    }
}
