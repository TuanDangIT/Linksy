using Linksy.Application.Abstractions;
using Linksy.Application.Urls.Features.RedirectToOriginalUrl;
using Linksy.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Handlers
{
    internal class RedirectToOriginalUrlHandler : IQueryHandler<RedirectToOriginalUrl, OriginalUrlDto>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly ILogger<RedirectToOriginalUrlHandler> _logger;

        public RedirectToOriginalUrlHandler(LinksyDbContext dbContext, ILogger<RedirectToOriginalUrlHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<OriginalUrlDto> Handle(RedirectToOriginalUrl request, CancellationToken cancellationToken)
        {
            var url = await _dbContext.Urls
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Code == request.Code, cancellationToken) ?? throw new UrlNotFoundException(request.Code);
            url.IncrementVisitsCounter();
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Redirecting to original URL: {OriginalUrl} for code: {Code}.", url.OriginalUrl, request.Code);
            return new OriginalUrlDto(url.OriginalUrl);
        }
    }
}
