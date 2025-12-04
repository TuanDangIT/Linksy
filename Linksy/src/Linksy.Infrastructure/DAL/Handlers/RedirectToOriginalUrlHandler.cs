using Azure.Core;
using Linksy.Application.Abstractions;
using Linksy.Application.Shared.DTO;
using Linksy.Application.Urls.Exceptions;
using Linksy.Application.Urls.Features.RedirectToOriginalUrl;
using Linksy.Domain.Entities.Tracking;
using Linksy.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Handlers
{
    internal class RedirectToOriginalUrlHandler : IQueryHandler<RedirectToOriginalUrl, RedirectToOriginalUrlResponse>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly ILogger<RedirectToOriginalUrlHandler> _logger;

        public RedirectToOriginalUrlHandler(LinksyDbContext dbContext, ILogger<RedirectToOriginalUrlHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<RedirectToOriginalUrlResponse> Handle(RedirectToOriginalUrl request, CancellationToken cancellationToken)
        {
            var url = await _dbContext.Urls
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Code == request.Code && u.IsActive == true, cancellationToken) ?? throw new UrlNotFoundException(request.Code);
            if(url.IsActive is false)
            {
                throw new UrlIsNotActiveException(url.Id);
            }
            url.IncrementVisitsCounter();
            url.AddEngagement(UrlEngagement.CreateEngagement(url, request.IpAddress));

            await AddEngagementAndIncreaseVisitCountForUmtParameterIfExists(request, url.Id, cancellationToken);
            await AddEngagementAndIncreaseScanCountForScanCodeIfExists(request, url.Id, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Redirecting to original URL: {OriginalUrl} for code: {Code}.", url.OriginalUrl, request.Code);
            return new RedirectToOriginalUrlResponse(url.OriginalUrl);
        }

        private async Task AddEngagementAndIncreaseVisitCountForUmtParameterIfExists(RedirectToOriginalUrl request, int urlId, CancellationToken cancellationToken = default)
        {
            if (request.UmtParameter is not null)
            {
                var umtParameter = await _dbContext.UmtParameters
                    .IgnoreQueryFilters()
                    .Include(u => u.Url)
                    .Where(u => u.UrlId == urlId &&
                        u.UmtSource == request.UmtParameter.UmtSource &&
                        u.UmtMedium == request.UmtParameter.UmtMedium &&
                        u.UmtCampaign == request.UmtParameter.UmtCampaign)
                    .FirstOrDefaultAsync(cancellationToken);

                if (umtParameter is not null)
                {
                    umtParameter.AddEngagement(UmtParameterEngagement.CreateUmtEngagementParameter(umtParameter, request.IpAddress));
                    umtParameter.IncrementVisits();
                }
            }
        }

        private async Task AddEngagementAndIncreaseScanCountForScanCodeIfExists(RedirectToOriginalUrl request, int urlId, CancellationToken cancellationToken = default)
        {
            if (request.IsBarcode is true)
            {
                var barcode = await _dbContext.Barcodes
                    .IgnoreQueryFilters()
                    .Include(s => s.Url)
                    .Where(s => s.UrlId == urlId)
                    .FirstOrDefaultAsync(cancellationToken);
                barcode?.AddEngagement(BarcodeEngagement.CreateBarcodeEngagement(barcode, request.IpAddress));
            }

            if (request.IsQrCode is true)
            {
                var query = _dbContext.QrCodes
                    .IgnoreQueryFilters()
                    .AsQueryable();
                if (request.UmtParameter is not null)
                {
                    query = query
                        .Include(q => q.UmtParameter)
                        .Where(q => q.UmtParameter != null &&
                            q.UmtParameter.UmtSource == request.UmtParameter.UmtSource &&
                            q.UmtParameter.UmtMedium == request.UmtParameter.UmtMedium &&
                            q.UmtParameter.UmtCampaign == request.UmtParameter.UmtCampaign);
                }
                else
                {
                    query = query
                        .Include(q => q.Url)
                        .Where(q => q.UrlId == urlId);
                }
                var qrCode = await query.FirstOrDefaultAsync(cancellationToken);
                qrCode?.AddEngagement(QrCodeEngagement.CreateQrCodeEngagement(qrCode, request.IpAddress));
            }
        }
    }
}
