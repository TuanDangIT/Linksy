using Linksy.Application.Abstractions;
using Linksy.Application.LandingPages.Features.BrowseLandingPages;
using Linksy.Domain.Entities.LandingPage;
using Linksy.Infrastructure.Pagination.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Handlers
{
    internal class BrowseLandingPagesHandler : IQueryHandler<BrowseLandingPages, BrowseLandingPagesResponse>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly IPaginationService<LandingPage> _paginationService;
        private readonly IContextService _contextService;
        private readonly ILogger<BrowseLandingPagesHandler> _logger;

        public BrowseLandingPagesHandler(LinksyDbContext dbContext, IPaginationService<LandingPage> paginationService, IContextService contextService,
            ILogger<BrowseLandingPagesHandler> logger)
        {
            _dbContext = dbContext;
            _paginationService = paginationService;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<BrowseLandingPagesResponse> Handle(BrowseLandingPages request, CancellationToken cancellationToken)
        {
            var query = _dbContext.LandingPages
                .AsNoTracking()
                .AsQueryable();

            var result = await _paginationService.PaginateAsync(query, request.PageNumber, request.PageSize, request.Filters, request.Orders,
                lp => new BrowseLandingPageDto(lp.Id, lp.Code, lp.IsPublished, lp.EngagementCount, lp.ViewCount, lp.Title, lp.TagsList, lp.CreatedAt, lp.UpdatedAt), cancellationToken);

            _logger.LogInformation("Browsed landing pages by user with ID {UserId}.", _contextService.Identity!.Id);
            return new BrowseLandingPagesResponse(result);
        }
    }
}
