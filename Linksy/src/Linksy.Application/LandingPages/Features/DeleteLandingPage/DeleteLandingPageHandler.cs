using Linksy.Application.Abstractions;
using Linksy.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.LandingPages.Features.DeleteLandingPage
{
    internal class DeleteLandingPageHandler : ICommandHandler<DeleteLandingPage>
    {
        private readonly ILandingPageRepository _landingPageRepository;
        private readonly IContextService _contextService;
        private readonly ILogger<DeleteLandingPageHandler> _logger;

        public DeleteLandingPageHandler(ILandingPageRepository landingPageRepository, IContextService contextService, ILogger<DeleteLandingPageHandler> logger)
        {
            _landingPageRepository = landingPageRepository;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task Handle(DeleteLandingPage request, CancellationToken cancellationToken)
        {
            await _landingPageRepository.DeleteAsync(request.LandingPageId, cancellationToken);
            _logger.LogInformation("Landing page with ID {LandingPageId} deleted by user {UserId}", request.LandingPageId, _contextService.Identity!.Id);
        }
    }
}
