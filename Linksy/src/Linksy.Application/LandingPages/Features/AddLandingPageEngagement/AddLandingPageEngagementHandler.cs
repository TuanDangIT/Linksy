using Linksy.Application.Abstractions;
using Linksy.Application.LandingPageItems.Exceptions;
using Linksy.Application.LandingPages.Exceptions;
using Linksy.Domain.Entities.Tracking;
using Linksy.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.AddLandingPageEngagement
{
    internal class AddLandingPageEngagementHandler : ICommandHandler<AddLandingPageEngagement>
    {
        private readonly ILandingPageRepository _landingPageRepository;
        private readonly ILogger<AddLandingPageEngagementHandler> _logger;

        public AddLandingPageEngagementHandler(ILandingPageRepository landingPageRepository, ILogger<AddLandingPageEngagementHandler> logger)
        {
            _landingPageRepository = landingPageRepository;
            _logger = logger;
        }
        public async Task Handle(AddLandingPageEngagement request, CancellationToken cancellationToken)
        {
            var landingPage = await _landingPageRepository.GetByIdWithoutQueryFilterAsync(request.LandingPageId, cancellationToken, lp => lp.LandingPageItems) ?? 
                throw new LandingPageNotFoundException(request.LandingPageId);
            var landingPageItem = landingPage.LandingPageItems.FirstOrDefault(lpi => lpi.Id == request.LandingPageItemId) ?? 
                throw new LandingPageItemNotFoundException(request.LandingPageItemId);

            landingPage.AddEngagement(LandingPageEngagement.CreateLandingPageEngagement(landingPage, request.IpAddress));
            landingPageItem.IncrementClickCount();

            await _landingPageRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Added engagement to landing page with ID {LandingPageId} from IP {IpAddress}.", request.LandingPageId, request.IpAddress);
        }
    }
}
