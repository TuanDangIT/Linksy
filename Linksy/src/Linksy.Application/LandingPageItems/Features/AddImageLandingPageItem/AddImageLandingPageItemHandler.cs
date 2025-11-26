using Linksy.Application.Abstractions;
using Linksy.Application.LandingPages.Exceptions;
using Linksy.Application.Shared.BlobStorage;
using Linksy.Application.Shared.Configuration;
using Linksy.Application.Urls.Exceptions;
using Linksy.Domain.DomainServices;
using Linksy.Domain.Entities.LandingPage;
using Linksy.Domain.Entities.Url;
using Linksy.Domain.Enums;
using Linksy.Domain.Repositories;
using Linksy.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPageItems.Features.AddImageLandingPageItem
{
    internal class AddImageLandingPageItemHandler : ICommandHandler<AddImageLandingPageItem, AddImageLandingPageItemResponse>
    {
        private readonly IContextService _contextService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly LinksyConfig _linksyConfig;
        private readonly TimeProvider _timeProvider;
        private readonly IUrlRepository _urlRepository;
        private readonly ILandingPageRepository _landingPageRepository;
        private readonly ILogger<AddImageLandingPageItemHandler> _logger;
        private readonly string _contentType = "image/png";
        private readonly string _pngExtension = ".png"; 

        public AddImageLandingPageItemHandler(IContextService contextService, IBlobStorageService blobStorageService, LinksyConfig linksyConfig, TimeProvider timeProvider,
            IUrlRepository urlRepository, ILandingPageRepository landingPageRepository, ILogger<AddImageLandingPageItemHandler> logger)
        {
            _contextService = contextService;
            _blobStorageService = blobStorageService;
            _linksyConfig = linksyConfig;
            _timeProvider = timeProvider;
            _urlRepository = urlRepository;
            _landingPageRepository = landingPageRepository;
            _logger = logger;
        }
        public async Task<AddImageLandingPageItemResponse> Handle(AddImageLandingPageItem request, CancellationToken cancellationToken)
        {
            var userId = _contextService.Identity!.Id;
            var landingPage = await _landingPageRepository.GetByIdAsync(request.LandingPageId, cancellationToken, lp => lp.LandingPageItems) ?? throw new LandingPageNotFoundException(request.LandingPageId);

            var entityName = nameof(LandingPageItem).ToLower();
            var timestamp = _timeProvider.GetUtcNow().ToString("yyyyMMddTHHmmssZ");
            var fileName = _linksyConfig.BlobStorage.ImageLandingPageItemPathFromContainer + $"{entityName}-{timestamp}{_pngExtension}";
            var containerName = $"user-{userId}";
            var stream = new MemoryStream();
            await request.Image.CopyToAsync(stream, cancellationToken);
            var file = new FormFile(stream, 0, stream.Length, entityName, fileName)
            {
                Headers = new HeaderDictionary()
            };
            file.ContentType = _contentType;
            var imageUrlPath = await _blobStorageService.UploadAsync(file, file.FileName, containerName);
            Url? url = null;
            if (request.UrlId is not null)
            {
                url = await _urlRepository.GetUrlAsync((int)request.UrlId, cancellationToken);
            }
            var imageLandingPageItem = ImageLandingPageItem.CreateImageLandingPageItem(LandingPageItemType.Image, new Image(imageUrlPath, fileName), request.AltText, url, userId);
            landingPage.AddLandingPageItem(imageLandingPageItem);
            await _landingPageRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("User {UserId} added ImageLandingPageItem {LandingPageItemId} to LandingPage {LandingPageId}.", userId, imageLandingPageItem.Id, landingPage.Id);
            return new AddImageLandingPageItemResponse(imageLandingPageItem.Id, imageUrlPath, fileName);
        }
    }
}
