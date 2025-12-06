using Linksy.Application.Abstractions;
using Linksy.Application.Statistics.Analytics;
using Linksy.Application.Statistics.Features.GetBarodeEngagementAnalytics;
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
    internal class GetBarcodeEngagementAnalyticsHandler : IQueryHandler<GetBarcodeEngagementAnalytics, AnalyticsResponse>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly IAnalyticsService _analyticsService;
        private readonly IContextService _contextService;
        private readonly ILogger<GetBarcodeEngagementAnalyticsHandler> _logger;

        public GetBarcodeEngagementAnalyticsHandler(LinksyDbContext dbContext, IAnalyticsService analyticsService, IContextService contextService,
            ILogger<GetBarcodeEngagementAnalyticsHandler> logger)
        {
            _dbContext = dbContext;
            _analyticsService = analyticsService;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<AnalyticsResponse> Handle(GetBarcodeEngagementAnalytics request, CancellationToken cancellationToken)
        {
            var doesBarcodeExist = await _dbContext.Barcodes.AsNoTracking().AnyAsync(b => b.Id == request.BarcodeId, cancellationToken);
            if (doesBarcodeExist is false)
            {
                throw new BarcodeNotFoundException(request.BarcodeId);
            }
            var query = _dbContext.BarcodeEngagements
                .Where(b => b.BarcodeId == request.BarcodeId)
                .AsNoTracking()
                .AsQueryable();
            var analytics = await _analyticsService.GetEngagementAnalyticsAsync(query, request, cancellationToken);
            _logger.LogInformation("Retrieved barcode engagement analytics for BarcodeId: {BarcodeId} by UserId: {UserId}.", request.BarcodeId, _contextService.Identity!.Id);
            return analytics;

        }
    }
}
