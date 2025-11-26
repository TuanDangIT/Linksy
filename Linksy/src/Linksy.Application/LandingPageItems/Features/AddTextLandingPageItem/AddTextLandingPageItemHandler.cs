using Linksy.Application.Abstractions;
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

namespace Linksy.Application.LandingPageItems.Features.AddTextLandingPageItem
{
    internal class AddTextLandingPageItemHandler : ICommandHandler<AddTextLandingPageItem, AddLandingPageResponse>
    {
        private readonly ILandingPageRepository _landingPageRepository;
        private readonly IContextService _contextService;
        private readonly ILogger<AddTextLandingPageItemHandler> _logger;

        public AddTextLandingPageItemHandler(ILandingPageRepository landingPageRepository, IContextService contextService, ILogger<AddTextLandingPageItemHandler> logger)
        {
            _landingPageRepository = landingPageRepository;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<AddLandingPageResponse> Handle(AddTextLandingPageItem request, CancellationToken cancellationToken)
        {
            var userId = _contextService.Identity!.Id;
            var landingPage = await _landingPageRepository.GetByIdAsync(request.LandingPageId, cancellationToken, lp => lp.LandingPageItems) ?? throw new LandingPageNotFoundException(request.LandingPageId);
            var textLandingPageItem = TextLandingPageItem.CreateTextLandingPageItem(LandingPageItemType.Text, request.Content, request.BackgroundColor, request.FontColor, userId);
            landingPage.AddLandingPageItem(textLandingPageItem);
            await _landingPageRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("User {UserId} added TextLandingPageItem {LandingPageItemId} to LandingPage {LandingPageId}.", userId, textLandingPageItem.Id, landingPage.Id);
            return new AddLandingPageResponse(textLandingPageItem.Id);
        }
    }
}
