using Linksy.Application.Abstractions;
using Linksy.Application.LandingPages.Exceptions;
using Linksy.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.PublishLandingPage
{
    internal class PublishLandingPageHandler : ICommandHandler<PublishLandingPage>
    {
        private readonly ILandingPageRepository _landingPageRepository;
        private readonly IContextService _contextService;
        private readonly ILogger<PublishLandingPageHandler> _logger;

        public PublishLandingPageHandler(ILandingPageRepository landingPageRepository, IContextService contextService, ILogger<PublishLandingPageHandler> logger)
        {
            _landingPageRepository = landingPageRepository;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task Handle(PublishLandingPage request, CancellationToken cancellationToken)
        {
            var landingPage = await _landingPageRepository.GetByIdAsync(request.LandingPageId, cancellationToken) ?? throw new LandingPageNotFoundException(request.LandingPageId);

            if (request.IsPublished)
            {
                landingPage.Publish();
            }
            else
            {
                landingPage.Unpublish();
            }
            await _landingPageRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Landing page with ID {LandingPageId} published status changed to {IsPublished} by user {UserId}", 
                request.LandingPageId, request.IsPublished, _contextService.Identity!.Id);
        }
    }
}
