using Linksy.Application.Abstractions;
using Linksy.Application.Shared.DTO;
using Linksy.Application.UmtParameters.Features.GetUmtParameter;
using Linksy.Infrastructure.Contexts;
using Linksy.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Handlers
{
    internal class GetUmtParameterHandler : IQueryHandler<GetUmtParameter, GetUmtParameterResponse?>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly IContextService _contextService;
        private readonly ILogger<GetBarcodeHandler> _logger;

        public GetUmtParameterHandler(LinksyDbContext dbContext, IContextService contextService, ILogger<GetBarcodeHandler> logger)
        {
            _dbContext = dbContext;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<GetUmtParameterResponse?> Handle(GetUmtParameter request, CancellationToken cancellationToken)
        {
            var url = await _dbContext.Urls
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == request.UrlId, cancellationToken) ?? throw new UrlNotFoundException(request.UrlId);

            var umtParameter = await _dbContext.UmtParameters
                .AsNoTracking()
                .Where(umt => umt.Id == request.UmtParameterId && umt.UrlId == request.UrlId)
                .Select(u => new GetUmtParameterResponse(
                    u.Id,
                    u.IsActive,
                    u.UmtSource,
                    u.UmtMedium,
                    u.UmtCampaign,
                    u.VisitCount,
                    new GetUmtParameterUrlDto(u.UrlId, u.Url.OriginalUrl, u.Url.Code),
                    u.QrCode != null ? new GetUmtParameterQrCodeDto(u.QrCode.Id, u.QrCode.ScanCount, new ImageDto(u.QrCode.ScanCodeImage.UrlPath, u.QrCode.ScanCodeImage.FileName)) : null,
                    u.CreatedAt,
                    u.UpdatedAt))
                .FirstOrDefaultAsync(cancellationToken);

            _logger.LogInformation("UmtParameter with ID: {UmtParameterId} retrieved successfully by user with ID: {UserId}.", request.UmtParameterId, _contextService.Identity!.Id);
            return umtParameter;
        }
    }
}
