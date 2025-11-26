using Linksy.Application.Abstractions;
using Linksy.Application.LandingPages.Features.GetLandingPage;
using Linksy.Application.LandingPages.Features.GetPublishedLandingPage;
using Linksy.Application.Shared.DTO;
using Linksy.Domain.Entities.LandingPage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Handlers
{
    internal class GetLandingPageHandler : IQueryHandler<GetLandingPage, GetLandingPageResponse?>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly IContextService _contextService;
        private readonly ILogger<GetLandingPageHandler> _logger;

        public GetLandingPageHandler(LinksyDbContext dbContext, IContextService contextService, ILogger<GetLandingPageHandler> logger)
        {
            _dbContext = dbContext;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<GetLandingPageResponse?> Handle(GetLandingPage request, CancellationToken cancellationToken)
        {
            var landingPage = await _dbContext.LandingPages
                .Where(lp => lp.Id == request.LandingPageId)
                .Include(lp => lp.LandingPageItems)
                .FirstOrDefaultAsync(cancellationToken);

            if (landingPage == null)
                return null;

            await _dbContext.Entry(landingPage)
                .Collection(lp => lp.LandingPageItems)
                .Query()
                .OfType<UrlLandingPageItem>()
                .Include(lpi => lpi.Url)
                .LoadAsync(cancellationToken);

            await _dbContext.Entry(landingPage)
                .Collection(lp => lp.LandingPageItems)
                .Query()
                .OfType<ImageLandingPageItem>()
                .Include(lpi => lpi.Url)
                .LoadAsync(cancellationToken);

            var response = new GetLandingPageResponse(
                landingPage.Code,
                landingPage.IsPublished,
                landingPage.EngagementCount,
                landingPage.ViewCount,
                landingPage.Title,
                landingPage.TitleFontColor,
                landingPage.Description,
                landingPage.DescriptionFontColor,
                landingPage.LogoImage != null ? new ImageDto(landingPage.LogoImage.UrlPath, landingPage.LogoImage.FileName) : null,
                landingPage.BackgroundColor,
                landingPage.BackgroundImage != null ? new ImageDto(landingPage.BackgroundImage.UrlPath, landingPage.BackgroundImage.FileName) : null,
                landingPage.TagsList,
                landingPage.CreatedAt,
                landingPage.UpdatedAt,
                landingPage.LandingPageItems.Select<LandingPageItem, object>(lpi =>
                {
                    return lpi switch
                    {
                        UrlLandingPageItem urlItem => new GetUrlLandingPageItemDto(
                            urlItem.Type.ToString(),
                            urlItem.Order,
                            urlItem.ClickCount,
                            urlItem.CreatedAt,
                            urlItem.UpdatedAt,
                            urlItem.Content,
                            urlItem.BackgroundColor,
                            urlItem.FontColor,
                            urlItem.Url.Code
                        ),
                        TextLandingPageItem textItem => new GetTextLandingPageItemDto(
                            textItem.Type.ToString(),
                            textItem.Order,
                            textItem.ClickCount,
                            textItem.CreatedAt,
                            textItem.UpdatedAt,
                            textItem.Content,
                            textItem.BackgroundColor,
                            textItem.FontColor
                        ),
                        ImageLandingPageItem imageItem => new GetImageLandingPageItemDto(
                            imageItem.Type.ToString(),
                            imageItem.Order,
                            imageItem.ClickCount,
                            imageItem.CreatedAt,
                            imageItem.UpdatedAt,
                            imageItem.Image.UrlPath,
                            imageItem.AltText,
                            imageItem.Url?.Code
                        ),
                        YouTubeLandingPageItem youTubeLandingPageItem => new GetYoutubeLandingPageItemDto(
                            youTubeLandingPageItem.Type.ToString(),
                            youTubeLandingPageItem.Order,
                            youTubeLandingPageItem.ClickCount,
                            youTubeLandingPageItem.CreatedAt,
                            youTubeLandingPageItem.UpdatedAt,
                            youTubeLandingPageItem.VideoUrl
                        ),
                        _ => throw new InvalidCastException("Unknown landing page item type.")
                    };
                })
            );

            _logger.LogInformation("Landing page with ID {LandingPageId} retrieved by user with ID: {UserId}.", request.LandingPageId, _contextService.Identity!.Id);

            return response;
        }
    }
}
