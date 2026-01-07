using Linksy.Application.Abstractions;
using Linksy.Application.LandingPages.Features.GetPublishedLandingPage;
using Linksy.Application.Shared.DTO;
using Linksy.Domain.Entities.LandingPage;
using Linksy.Domain.Entities.Tracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Handlers
{
    internal class GetPublishedLandingPageHandler : IQueryHandler<GetPublishedLandingPage, GetPublishedLandingPageResponse?>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly TimeProvider _timeProvider;
        private readonly ILogger<GetPublishedLandingPageHandler> _logger;

        public GetPublishedLandingPageHandler(LinksyDbContext dbContext, TimeProvider timeProvider, ILogger<GetPublishedLandingPageHandler> logger)
        {
            _dbContext = dbContext;
            _timeProvider = timeProvider;
            _logger = logger;
        }

        public async Task<GetPublishedLandingPageResponse?> Handle(GetPublishedLandingPage request, CancellationToken cancellationToken)
        {
            var landingPage = await _dbContext.LandingPages
                .Where(lp => lp.Code == request.Code && lp.IsPublished)
                .Include(lp => lp.LandingPageItems)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(cancellationToken);

            if (landingPage is null)
                return null;

            await _dbContext.Entry(landingPage)
                .Collection(lp => lp.LandingPageItems)
                .Query()
                .OfType<UrlLandingPageItem>()
                .Include(lpi => lpi.Url)
                .IgnoreQueryFilters()
                .LoadAsync(cancellationToken);

            await _dbContext.Entry(landingPage)
                .Collection(lp => lp.LandingPageItems)
                .Query()
                .OfType<ImageLandingPageItem>()
                .Include(lpi => lpi.Url)
                .IgnoreQueryFilters()
                .LoadAsync(cancellationToken);

            landingPage.AddView(LandingPageView.CreateLandingPageView(landingPage, request.IpAddress, _timeProvider.GetUtcNow().UtcDateTime));

            var response = new GetPublishedLandingPageResponse(
                landingPage.Id,
                landingPage.Title,
                landingPage.TitleFontColor,
                landingPage.Description,
                landingPage.DescriptionFontColor,
                landingPage.LogoImage != null ? new ImageDto(landingPage.LogoImage.UrlPath, landingPage.LogoImage.FileName) : null,
                landingPage.BackgroundColor,
                landingPage.BackgroundImage != null ? new ImageDto(landingPage.BackgroundImage.UrlPath, landingPage.BackgroundImage.FileName) : null,
                landingPage.LandingPageItems.Select<LandingPageItem, object>(lpi =>
                {
                    return lpi switch
                    {
                        UrlLandingPageItem urlItem => new GetPublishedUrlLandingPageItemDto(
                            urlItem.Id,
                            urlItem.Type.ToString(),
                            urlItem.Order,
                            urlItem.Content,
                            urlItem.BackgroundColor,
                            urlItem.FontColor,
                            urlItem.Url.Code
                        ),
                        TextLandingPageItem textItem => new GetPublishedTextLandingPageItemDto(
                            textItem.Id,
                            textItem.Type.ToString(),
                            textItem.Order,
                            textItem.Content,
                            textItem.BackgroundColor,
                            textItem.FontColor
                        ),
                        ImageLandingPageItem imageItem => new GetPublishedImageLandingPageItemDto(
                            imageItem.Id,
                            imageItem.Type.ToString(),
                            imageItem.Order,
                            imageItem.Image.UrlPath,
                            imageItem.AltText,
                            imageItem.Url?.Code
                        ),
                        YouTubeLandingPageItem youTubeLandingPageItem => new GetPublishedYoutubeLandingPageItemDto(
                            youTubeLandingPageItem.Id,
                            youTubeLandingPageItem.Type.ToString(),
                            youTubeLandingPageItem.Order,
                            youTubeLandingPageItem.VideoUrl
                        ),
                        _ => throw new InvalidCastException("Unknown landing page item type.")
                    };
                })
                );

            _logger.LogInformation("Published landing page with Code {LandingPageCode} retrieved.", request.Code);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return response;
        }
    }
}
