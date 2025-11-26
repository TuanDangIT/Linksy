using Linksy.Application.Abstractions;
using Linksy.Application.Shared.BlobStorage;
using Linksy.Domain.Entities.LandingPage;
using Linksy.Domain.Enums;
using Linksy.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPageItems.Features.DeleteLandingPageItem
{
    internal class DeleteLandingPageItemHandler : ICommandHandler<DeleteLandingPageItem>
    {
        private readonly ILandingPageItemRepository _landingPageItemRepository;
        private readonly IContextService _contextService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ILogger<DeleteLandingPageItemHandler> _logger;

        public DeleteLandingPageItemHandler(ILandingPageItemRepository landingPageItemRepository, IContextService contextService, IBlobStorageService blobStorageService, 
            ILogger<DeleteLandingPageItemHandler> logger)
        {
            _landingPageItemRepository = landingPageItemRepository;
            _contextService = contextService;
            _blobStorageService = blobStorageService;
            _logger = logger;
        }

        public async Task Handle(DeleteLandingPageItem request, CancellationToken cancellationToken)
        {
            var landingPageItem = await _landingPageItemRepository.GetByIdAsync(request.LandingPageItemId, cancellationToken);
            if (landingPageItem is null)
            {
                return;
            }

            if (landingPageItem.Type is LandingPageItemType.Image)
            {
                var imageLandingPageItem = (ImageLandingPageItem)landingPageItem;
                if (imageLandingPageItem.Image is not null)
                {
                    await _blobStorageService.DeleteAsync(imageLandingPageItem.Image.FileName, $"user-{_contextService.Identity!.Id}");
                }
            }
            await _landingPageItemRepository.DeleteAsync(request.LandingPageItemId, cancellationToken);
            _logger.LogInformation("Landing page item with ID {LandingPageItemId} deleted by user {UserId}", request.LandingPageItemId, _contextService.Identity!.Id);
        }
    }
}
