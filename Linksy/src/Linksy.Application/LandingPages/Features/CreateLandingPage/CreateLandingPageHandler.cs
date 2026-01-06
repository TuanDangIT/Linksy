using Linksy.Application.Abstractions;
using Linksy.Application.LandingPages.Exceptions;
using Linksy.Application.Shared.BlobStorage;
using Linksy.Domain.Entities.LandingPage;
using Linksy.Domain.Repositories;
using Linksy.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.CreateLandingPage
{
    internal class CreateLandingPageHandler : ICommandHandler<CreateLandingPage, CreateLandingPageResponse>
    {
        private readonly ILandingPageRepository _landingPageRepository;
        private readonly IContextService _contextService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ILogger<CreateLandingPageHandler> _logger;
        private string _containerName;

        public CreateLandingPageHandler(ILandingPageRepository landingPageRepository, IContextService contextService, IBlobStorageService blobStorageService, ILogger<CreateLandingPageHandler> logger)
        {
            _landingPageRepository = landingPageRepository;
            _contextService = contextService;
            _blobStorageService = blobStorageService;
            _logger = logger;
            _containerName = $"user-{_contextService.Identity!.Id}";
        }
        public async Task<CreateLandingPageResponse> Handle(CreateLandingPage request, CancellationToken cancellationToken)
        {
            if(await _landingPageRepository.IsLandingPageCodeInUseAsync(request.Code, cancellationToken))
            {
                throw new LandingPageCodeInUseException(request.Code);
            }

            var userId = _contextService.Identity!.Id;
            Image? logoImage = null;
            Image? backgroundImage = null;

            if (request.LogoImage is not null)
            {
                var fileName = "landing-pages/logos/" + request.LogoImage.FileName;
                var logoUrlPath = await _blobStorageService.UploadAsync(request.LogoImage, fileName, _containerName, cancellationToken);
                logoImage = new Image(logoUrlPath, fileName);
            }

            if (request.BackgroundImage is not null)
            {
                var fileName = "landing-pages/background-images/" + request.BackgroundImage.FileName;
                var backgroundImageUrl = await _blobStorageService.UploadAsync(request.BackgroundImage, fileName, _containerName, cancellationToken);
                backgroundImage = new Image(backgroundImageUrl, fileName);
            }

            var landingPage = LandingPage.CreateLandingPage(request.Code, request.Title, request.TitleFontColor,
                request.Description, request.DescriptionFontColor, logoImage,  request.BackgroundColor, backgroundImage, request.Tags, userId);

            await _landingPageRepository.CreateAsync(landingPage, cancellationToken);

            _logger.LogInformation("Landing page created: {LandingPageCode} by user with ID: {userId}.", request.Code, userId);

            return new CreateLandingPageResponse(landingPage.Id);
        }
    }
}
