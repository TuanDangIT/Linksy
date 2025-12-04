using Linksy.Application.Abstractions;
using Linksy.Application.Statistics.Features.GetMediumCountsForUmtParameter;
using Linksy.Application.Statistics.Features.GetSourceCountsForUmtParameter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Handlers
{
    internal class GetSourceCountsForUmtParameterHandler : IQueryHandler<GetSourceCountsForUmtParameter, GetSourceCountsForUmtParameterResponse>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly IContextService _contextService;
        private readonly ILogger<GetSourceCountsForUmtParameterHandler> _logger;

        public GetSourceCountsForUmtParameterHandler(LinksyDbContext dbContext, IContextService contextService, ILogger<GetSourceCountsForUmtParameterHandler> logger)
        {
            _dbContext = dbContext;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<GetSourceCountsForUmtParameterResponse> Handle(GetSourceCountsForUmtParameter request, CancellationToken cancellationToken)
        {
            var response = await _dbContext.Urls
                .Where(u => u.Id == request.UrlId)
                .SelectMany(u => u.UmtParameters)
                .Where(up => up.UmtSource != null)
                .GroupBy(up => up.UmtSource)
                .Select(up => new SourceCountDto(up.Key!, up.Sum(u => u.VisitCount)))
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved source counts for UMT parameter for UrlId: {UrlId} by UserId: {UserId}.", request.UrlId, _contextService.Identity!.Id);
            return new GetSourceCountsForUmtParameterResponse(response);
        }
    }
}
