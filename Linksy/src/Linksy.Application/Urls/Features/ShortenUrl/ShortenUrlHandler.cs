using Linksy.Application.Abstractions;
using Linksy.Application.Shared.Configuration;
using Linksy.Application.Urls.Exceptions;
using Linksy.Domain.DomainServices;
using Linksy.Domain.Entities;
using Linksy.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.ShortenUrl
{
    internal class ShortenUrlHandler : ICommandHandler<ShortenUrl, ShortenedUrlResponse>
    {
        private readonly IGenerateShotenedUrlService _generateShotenedUrlService;
        private readonly IUrlRepository _urlRepository;
        private readonly LinksyConfig _linksyConfig;
        private readonly IContextService _contextService;
        private readonly ILogger<ShortenUrlHandler> _logger;

        public ShortenUrlHandler(IGenerateShotenedUrlService generateShotenedUrlService, IUrlRepository urlRepository, 
            LinksyConfig linksyConfig, IContextService contextService, ILogger<ShortenUrlHandler> logger)
        {
            _generateShotenedUrlService = generateShotenedUrlService;
            _urlRepository = urlRepository;
            _linksyConfig = linksyConfig;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<ShortenedUrlResponse> Handle(ShortenUrl request, CancellationToken cancellationToken)
        {
            var umtParameters = request.UmtParameters?.Select(u => UmtParameter.CreateUmtParameter(u.UmtSource, u.UmtMedium, u.UmtCampaign));
            var userId = _contextService.Identity!.Id;
            var url = await _generateShotenedUrlService.GenerateShortenedUrl(request.OriginalUrl, request.CustomCode, umtParameters, userId, cancellationToken);
            await _urlRepository.CreateAsync(url, cancellationToken);
            var shortenedUrl = _linksyConfig.BaseUrl + "/" + url.Code;
            _logger.LogInformation("URL shortened: {OriginalUrl} -> {ShortenedCode} by user with ID: {userId}.", request.OriginalUrl, shortenedUrl, userId);
            return new ShortenedUrlResponse(url.Id, shortenedUrl);
        }
    }
}
