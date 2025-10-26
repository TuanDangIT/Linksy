using Linksy.Application.Abstractions;
using Linksy.Application.Urls.Exceptions;
using Linksy.Domain.Entities;
using Linksy.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.AddUmtParameterToUrl
{
    internal class AddUmtParameterToUrlHandler : ICommandHandler<AddUmtParameterToUrl>
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
        public async Task Handle(AddUmtParameterToUrl request, CancellationToken cancellationToken)
        {
            var url = await _urlRepository.GetUrlAsync(request.UrlId, cancellationToken, u => u.QrCode) ?? throw new UrlNotFoundException(request.UrlId);
            var umtParameter = UmtParameter.CreateUmtParameter(request.UmtParameter.UmtSource, request.UmtParameter.UmtMedium, request.UmtParameter.UmtCampaign);
            url.AddUmtParameter(umtParameter);
            await _urlRepository.UpdateAsync(cancellationToken);
        }
    }
}
