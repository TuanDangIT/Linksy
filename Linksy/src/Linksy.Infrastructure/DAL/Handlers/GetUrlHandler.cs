using Linksy.Application.Abstractions;
using Linksy.Application.Urls.Features.GetUrl;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linksy.Infrastructure.Exceptions;

namespace Linksy.Infrastructure.DAL.Handlers
{
    internal class GetUrlHandler : IQueryHandler<GetUrl, GetUrlDto?>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly IContextService _contextService;
        private readonly ILogger<GetUrlHandler> _logger;

        public GetUrlHandler(LinksyDbContext dbContext, IContextService contextService, ILogger<GetUrlHandler> logger)
        {
            _dbContext = dbContext;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<GetUrlDto?> Handle(GetUrl request, CancellationToken cancellationToken)
        {
            var url = await _dbContext.Urls
                .Include(u => u.QrCode)
                .Include(u => u.Barcode)
                .Include(u => u.LandingPage)
                .Include(u => u.LandingPageItem)
                .ThenInclude(lp => lp!.LandingPage)
                .Include(u => u.UmtParameters!)
                .ThenInclude(up => up.QrCode)
                .AsNoTracking()
                .Select(u => new GetUrlDto(u.Id, u.OriginalUrl, u.Code, u.VisitCount, u.IsActive,
                    u.QrCode == null ? null : new GetUrlQrCodeDto(u.QrCode.Id, u.QrCode.ImageUrlPath, u.QrCode.ScanCount, u.QrCode.CreatedAt, u.QrCode.UpdatedAt),
                    u.Barcode == null ? null : new GetUrlBarcodeDto(u.Barcode.Id, u.Barcode.ImageUrlPath, u.Barcode.ScanCount, u.Barcode.CreatedAt, u.Barcode.UpdatedAt),
                    u.LandingPage == null ? null : new GetUrlLandingPageDto(u.LandingPage.Id, u.LandingPage.Title, u.LandingPage.CreatedAt, u.LandingPage.UpdatedAt),
                    u.LandingPageItem == null ? null : new GetUrlLandingPageItemDto(u.LandingPageItem.Id, u.LandingPage!.Id, u.LandingPage!.Title, u.LandingPageItem.CreatedAt, u.LandingPageItem.UpdatedAt),
                    u.UmtParameters!.Select(up => new GetUrlUmtParameterDto(up.Id, up.UmtSource, up.UmtMedium, up.UmtCampaign, up.QrCode != null ? up.QrCode.Id : null, up.QrCode != null ? up.QrCode.Id : null, up.CreatedAt, up.UpdatedAt))))
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken) ?? throw new UrlNotFoundException(request.Id);
            _logger.LogInformation("Retrieved URL with ID: {urlId} for user with ID: {}.", request.Id, _contextService.Identity!.Id);
            return url;
        }
    }
}
