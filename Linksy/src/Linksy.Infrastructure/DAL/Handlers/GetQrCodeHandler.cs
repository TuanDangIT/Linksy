using Linksy.Application.Abstractions;
using Linksy.Application.QrCodes.Features.GetQrCode;
using Linksy.Application.Shared.DTO;
using Linksy.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.DAL.Handlers
{
    internal class GetQrCodeHandler : IQueryHandler<GetQrCode, GetQrCodeResponse?>
    {
        private readonly LinksyDbContext _dbContext;
        private readonly IContextService _contextService;
        private readonly ILogger<GetBarcodeHandler> _logger;

        public GetQrCodeHandler(LinksyDbContext dbContext, IContextService contextService, ILogger<GetBarcodeHandler> logger)
        {
            _dbContext = dbContext;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<GetQrCodeResponse?> Handle(GetQrCode request, CancellationToken cancellationToken)
        {
            var qrCode = await _dbContext.QrCodes
                .AsNoTracking()
                .Where(q => q.Id == request.QrCodeId)
                .Select(q => new GetQrCodeResponse(
                    q.Id,
                    q.ScanCount,
                    new ImageDto(q.ScanCodeImage.UrlPath, q.ScanCodeImage.FileName),
                    q.TagsList,
                    q.CreatedAt,
                    q.UpdatedAt,
                    q.Url != null ? new GetQrCodeUrlDto(
                        q.Url.Id,
                        q.Url.OriginalUrl,
                        q.Url.Code) : null,
                    q.UmtParameter != null ? new GetQrCodeUmtParameterDto(
                        q.UmtParameter.Id,
                        q.UmtParameter.UmtSource,
                        q.UmtParameter.UmtMedium,
                        q.UmtParameter.UmtCampaign,
                        q.UmtParameter.UrlId,
                        q.UmtParameter.Url.OriginalUrl,
                        q.UmtParameter.Url.Code) : null
                ))
                .FirstOrDefaultAsync(cancellationToken);

            _logger.LogInformation("Retrieved qr code with ID: {QrCodeId} by user with ID: {UserId}.", request.QrCodeId, _contextService.Identity!.Id);
            return qrCode;
        }
    }
}
