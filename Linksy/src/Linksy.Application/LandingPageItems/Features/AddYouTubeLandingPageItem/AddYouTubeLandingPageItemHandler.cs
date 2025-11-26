using Linksy.Application.Abstractions;
using Linksy.Application.LandingPageItems.Features.AddTextLandingPageItem;
using Linksy.Application.LandingPages.Exceptions;
using Linksy.Application.Shared.DTO;
using Linksy.Domain.Entities.LandingPage;
using Linksy.Domain.Enums;
using Linksy.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPageItems.Features.AddYouTubeLandingPageItem
{
    internal class AddYouTubeLandingPageItemHandler : ICommandHandler<AddYouTubeLandingPageItem, AddLandingPageResponse>
    {
        private readonly ILandingPageRepository _landingPageRepository;
        private readonly IContextService _contextService;
        private readonly ILogger<AddTextLandingPageItemHandler> _logger;

        public AddYouTubeLandingPageItemHandler(ILandingPageRepository landingPageRepository, IContextService contextService, ILogger<AddTextLandingPageItemHandler> logger)
        {
            _landingPageRepository = landingPageRepository;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<AddLandingPageResponse> Handle(AddYouTubeLandingPageItem request, CancellationToken cancellationToken)
        {
            var userId = _contextService.Identity!.Id;
            var landingPage = await _landingPageRepository.GetByIdAsync(request.LandingPageId, cancellationToken, lp => lp.LandingPageItems) ?? throw new LandingPageNotFoundException(request.LandingPageId);
            var youTubeLandingPageItem = YouTubeLandingPageItem.CreateYouTubeLandingPageItem(LandingPageItemType.YouTube, request.YouTubeUrl, userId);
            landingPage.AddLandingPageItem(youTubeLandingPageItem);
            await _landingPageRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("User {UserId} added YouTubeLandingPageItem {LandingPageItemId} to LandingPage {LandingPageId}.", userId, youTubeLandingPageItem.Id, landingPage.Id);
            return new AddLandingPageResponse(youTubeLandingPageItem.Id);
        }
    }
}
