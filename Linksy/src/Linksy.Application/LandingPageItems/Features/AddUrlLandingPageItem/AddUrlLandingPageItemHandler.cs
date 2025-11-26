using Linksy.Application.Abstractions;
using Linksy.Application.LandingPageItems.Features.AddTextLandingPageItem;
using Linksy.Application.LandingPages.Exceptions;
using Linksy.Application.Shared.DTO;
using Linksy.Application.Urls.Exceptions;
using Linksy.Domain.DomainServices;
using Linksy.Domain.Entities.LandingPage;
using Linksy.Domain.Entities.Url;
using Linksy.Domain.Enums;
using Linksy.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPageItems.Features.AddUrlLandingPageItem
{
    internal class AddUrlLandingPageItemHandler : ICommandHandler<AddUrlLandingPageItem, AddLandingPageResponse>
    {
        private readonly ILandingPageRepository _landingPageRepository;
        private readonly IContextService _contextService;
        private readonly IUrlRepository _urlRepository;
        private readonly ILogger<AddTextLandingPageItemHandler> _logger;

        public AddUrlLandingPageItemHandler(ILandingPageRepository landingPageRepository, IContextService contextService, IUrlRepository urlRepository, 
            ILogger<AddTextLandingPageItemHandler> logger)
        {
            _landingPageRepository = landingPageRepository;
            _contextService = contextService;
            _urlRepository = urlRepository;
            _logger = logger;
        }

        public async Task<AddLandingPageResponse> Handle(AddUrlLandingPageItem request, CancellationToken cancellationToken)
        {
            var landingPage = await _landingPageRepository.GetByIdAsync(request.LandingPageId, cancellationToken, lp => lp.LandingPageItems) ?? throw new LandingPageNotFoundException(request.LandingPageId);
            var userId = _contextService.Identity!.Id;
            var url = await _urlRepository.GetUrlAsync(request.UrlId, cancellationToken) ?? throw new UrlNotFoundException(request.UrlId);
            var urlLandingPageItem = UrlLandingPageItem.CreateUrlLandingPageItem(LandingPageItemType.Url, request.Content, request.FontColor, request.BackgroundColor, url, userId);
            landingPage.AddLandingPageItem(urlLandingPageItem);
            await _landingPageRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("User {UserId} added UrlLandingPageItem {LandingPageItemId} to LandingPage {LandingPageId}.", userId, urlLandingPageItem.Id, landingPage.Id);
            return new AddLandingPageResponse(urlLandingPageItem.Id);
        }
    }
}
