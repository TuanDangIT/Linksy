using Linksy.Application.Abstractions;
using Linksy.Application.Statistics.Analytics;
using Linksy.Application.Statistics.Features.GetQrCodeEngagementAnalytics;
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
    internal class GetQrCodeEngagementAnalyticsHandler : IQueryHandler<GetQrCodeEngagementAnalytics, AnalyticsResponse>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly IAnalyticsService _analyticsService;
        private readonly IContextService _contextService;
        private readonly ILogger<GetQrCodeEngagementAnalyticsHandler> _logger;

        public GetQrCodeEngagementAnalyticsHandler(LinksyDbContext dbContext, IAnalyticsService analyticsService, IContextService contextService,
            ILogger<GetQrCodeEngagementAnalyticsHandler> logger)
        {
            _dbContext = dbContext;
            _analyticsService = analyticsService;
            _contextService = contextService;
            _logger = logger;
        }
        public Task<AnalyticsResponse> Handle(GetQrCodeEngagementAnalytics request, CancellationToken cancellationToken)
        {
            var doesQrCodeExist = _dbContext.QrCodes.AsNoTracking().Any(qr => qr.Id == request.QrCodeId);
            if (doesQrCodeExist is false)
            {
                throw new QrCodeNotFoundException(request.QrCodeId);
            }
            var query = _dbContext.QrCodeEngagements
                .Where(qr => qr.QrCodeId == request.QrCodeId)
                .AsNoTracking()
                .AsQueryable();
            var analytics = _analyticsService.GetEngagementAnalyticsAsync(query, request, cancellationToken);
            _logger.LogInformation("Retrieved qr code engagement analytics for QrCodeId: {QrCodeId} by UserId: {UserId}.", request.QrCodeId, _contextService.Identity!.Id);
            return analytics;
        }
    }
}
