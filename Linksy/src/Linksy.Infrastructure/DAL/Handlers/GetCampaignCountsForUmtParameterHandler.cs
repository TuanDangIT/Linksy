using Linksy.Application.Abstractions;
using Linksy.Application.Statistics.Features.GetCampaignCountsForUmtParameter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Handlers
{
    internal class GetCampaignCountsForUmtParameterHandler : IQueryHandler<GetCampaignCountsForUmtParameter, GetCampaignCountsForUmtParameterResponse>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly IContextService _contextService;
        private readonly ILogger<GetCampaignCountsForUmtParameterHandler> _logger;

        public GetCampaignCountsForUmtParameterHandler(LinksyDbContext dbContext, IContextService contextService, ILogger<GetCampaignCountsForUmtParameterHandler> logger)
        {
            _dbContext = dbContext;
            _contextService = contextService;
            _logger = logger;
        }

        public async Task<GetCampaignCountsForUmtParameterResponse> Handle(GetCampaignCountsForUmtParameter request, CancellationToken cancellationToken)
        {
            var response = await _dbContext.Urls
                .Where(u => u.Id == request.UrlId)
                .SelectMany(u => u.UmtParameters)
                .Where(up => up.UmtCampaign != null)
                .GroupBy(up => up.UmtCampaign)
                .Select(up => new CampaignCountDto(up.Key!, up.Sum(u => u.VisitCount)))
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved campaign counts for UTM parameters for UrlId: {UrlId} by UserId: {UserId}.", request.UrlId, _contextService.Identity!.Id);
            return new GetCampaignCountsForUmtParameterResponse(response);
        }
    }
}
