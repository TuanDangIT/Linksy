using Linksy.Application.Abstractions;
using Linksy.Application.Statistics.Features.GetCampaignCountsForUmtParameter;
using Linksy.Application.Statistics.Features.GetMediumCountsForUmtParameter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Handlers
{
    internal class GetMediumCountsForUmtParameterHandler : IQueryHandler<GetMediumCountsForUmtParameter, GetMediumCountsForUmtParameterResponse>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly IContextService _contextService;
        private readonly ILogger<GetMediumCountsForUmtParameterResponse> _logger;

        public GetMediumCountsForUmtParameterHandler(LinksyDbContext dbContext, IContextService contextService, ILogger<GetMediumCountsForUmtParameterResponse> logger)
        {
            _dbContext = dbContext;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<GetMediumCountsForUmtParameterResponse> Handle(GetMediumCountsForUmtParameter request, CancellationToken cancellationToken)
        {
            var response = await _dbContext.Urls
                .Where(u => u.Id == request.UrlId)
                .SelectMany(u => u.UmtParameters)
                .Where(up => up.UmtMedium != null)
                .GroupBy(up => up.UmtMedium)
                .Select(up => new MediumCountDto(up.Key!, up.Sum(u => u.VisitCount)))
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved medium counts for UMT parameters for UrlId: {UrlId} by UserId: {UserId}.", request.UrlId, _contextService.Identity!.Id);
            return new GetMediumCountsForUmtParameterResponse(response);
        }
    }
}
