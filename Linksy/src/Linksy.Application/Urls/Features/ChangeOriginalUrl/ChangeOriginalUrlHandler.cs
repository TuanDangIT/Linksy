using Linksy.Application.Abstractions;
using Linksy.Application.Urls.Exceptions;
using Linksy.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.ChangeOriginalUrl
{
    internal class ChangeOriginalUrlHandler : ICommandHandler<ChangeOriginalUrl>
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IContextService _contextService;
        private readonly ILogger<ChangeOriginalUrlHandler> _logger;

        public ChangeOriginalUrlHandler(IUrlRepository urlRepository, IContextService contextService, ILogger<ChangeOriginalUrlHandler> logger)
        {
            _urlRepository = urlRepository;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task Handle(ChangeOriginalUrl request, CancellationToken cancellationToken)
        {
            var url = await _urlRepository.GetUrlAsync(request.UrlId, cancellationToken) ?? throw new UrlNotFoundException(request.UrlId);
            url.ChangeOrginalUrl(request.OriginalUrl);
            _logger.LogInformation("Original URL changed to {OriginalUrl} for URL with ID: {UrlId} by user with ID: {}.", 
                request.OriginalUrl, request.UrlId, _contextService.Identity!.Id);
            await _urlRepository.UpdateAsync(cancellationToken);
        }
    }
}
