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
using Linksy.Application.Shared.DTO;

namespace Linksy.Infrastructure.DAL.Handlers
    {
        internal class GetUrlHandler : IQueryHandler<GetUrl, GetUrlResponse?>
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

            public async Task<GetUrlResponse?> Handle(GetUrl request, CancellationToken cancellationToken)
            {
                var url = await _dbContext.Urls
                    .Where(u => u.Id == request.Id)
                    .AsNoTracking()
                    .Select(u => new GetUrlResponse(
                        u.Id,
                        u.OriginalUrl,
                        u.Code,
                        u.VisitCount,
                        u.IsActive,
                        u.TagsList,
                        u.QrCode == null ? null : new GetUrlQrCodeDto(
                            u.QrCode.Id,
                            new ImageDto(u.QrCode.ScanCodeImage.UrlPath, u.QrCode.ScanCodeImage.FileName),
                            u.QrCode.ScanCount,
                            u.QrCode.CreatedAt,
                            u.QrCode.UpdatedAt),
                        u.Barcode == null ? null : new GetUrlBarcodeDto(
                            u.Barcode.Id,
                            new ImageDto(u.Barcode.ScanCodeImage.UrlPath, u.Barcode.ScanCodeImage.FileName),
                            u.Barcode.ScanCount,
                            u.Barcode.CreatedAt,
                            u.Barcode.UpdatedAt),
                        u.ImageLandingPageItems
                            .Select(i => new GetUrlLandingPageItemDto(
                                i.Id,
                                i.Type.ToString(),
                                i.ClickCount,
                                i.LandingPageId,
                                i.LandingPage.Title,
                                i.CreatedAt,
                                i.UpdatedAt)),
                        u.UmtParameters!.Select(up => new GetUrlUmtParameterDto(
                        up.Id,
                        up.UmtSource,
                        up.UmtMedium,
                        up.UmtCampaign,
                        up.QrCode != null ? up.QrCode.Id : null,
                        up.QrCode != null ? up.QrCode.ScanCount : null,  
                        up.CreatedAt,
                        up.UpdatedAt))))
                .FirstOrDefaultAsync(cancellationToken);
            _logger.LogInformation("Retrieved URL with ID: {urlId} for user with ID: {}.", request.Id, _contextService.Identity!.Id);
            return url;
        }
    }
}
