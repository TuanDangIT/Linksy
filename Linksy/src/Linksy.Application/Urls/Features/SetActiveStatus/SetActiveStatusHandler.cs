using Linksy.Application.Abstractions;
using Linksy.Application.Urls.Exceptions;
using Linksy.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.SetActiveStatus
{
    internal class SetActiveStatusHandler : ICommandHandler<SetActiveStatus>
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IContextService _contextService;
        private readonly ILogger<SetActiveStatusHandler> _logger;

        public SetActiveStatusHandler(IUrlRepository urlRepository, IContextService contextService, ILogger<SetActiveStatusHandler> logger)
        {
            _urlRepository = urlRepository;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task Handle(SetActiveStatus request, CancellationToken cancellationToken)
        {
            var url = await _urlRepository.GetUrlAsync(request.UrlId, cancellationToken) ?? throw new UrlNotFoundException(request.UrlId);
            url.SetActive(request.IsActive);
            _logger.LogInformation("Set active status to {IsActive} for URL with ID: {UrlId} by user with ID: {}.",
                request.IsActive, request.UrlId, _contextService.Identity!.Id);
            await _urlRepository.UpdateAsync(cancellationToken);
        }
    }
}
