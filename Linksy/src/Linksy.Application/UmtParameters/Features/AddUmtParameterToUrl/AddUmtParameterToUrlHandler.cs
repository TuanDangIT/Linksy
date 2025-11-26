using Linksy.Application.Abstractions;
using Linksy.Application.Urls.Exceptions;
using Linksy.Domain.Entities.Url;
using Linksy.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.UmtParameters.Features.AddUmtParameterToUrl
{
    internal class AddUmtParameterToUrlHandler : ICommandHandler<AddUmtParameterToUrl, AddUmtParameterToUrlResponse>
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IContextService _contextService;
        private readonly ILogger<AddUmtParameterToUrlHandler> _logger;

        public AddUmtParameterToUrlHandler(IUrlRepository urlRepository, IContextService contextService, ILogger<AddUmtParameterToUrlHandler> logger)
        {
            _urlRepository = urlRepository;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<AddUmtParameterToUrlResponse> Handle(AddUmtParameterToUrl request, CancellationToken cancellationToken)
        {
            //In progress with QR Code
            var userId = _contextService.Identity!.Id;
            var url = await _urlRepository.GetUrlAsync(request.UrlId, cancellationToken, u => u.UmtParameters) ?? throw new UrlNotFoundException(request.UrlId);
            var umtParameter = UmtParameter.CreateUmtParameter(request.UmtParameter.UmtSource, request.UmtParameter.UmtMedium, request.UmtParameter.UmtCampaign, userId);
            url.AddUmtParameter(umtParameter);
            await _urlRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("UMT parameter added to URL with ID: {UrlId} by user with ID: {UserId}.", request.UrlId, userId);
            return new AddUmtParameterToUrlResponse(umtParameter.Id);
        }
    }
}
