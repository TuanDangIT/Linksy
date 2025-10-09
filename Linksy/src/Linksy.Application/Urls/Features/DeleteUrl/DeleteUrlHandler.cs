using Linksy.Application.Abstractions;
using Linksy.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.DeleteUrl
{
    internal class DeleteUrlHandler : ICommandHandler<DeleteUrl>
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IContextService _contextService;
        private readonly ILogger<DeleteUrlHandler> _logger;

        public DeleteUrlHandler(IUrlRepository urlRepository, IContextService contextService, ILogger<DeleteUrlHandler> logger)
        {
            _urlRepository = urlRepository;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task Handle(DeleteUrl request, CancellationToken cancellationToken)
        {
            await _urlRepository.DeleteAsync(request.Id, cancellationToken);
            _logger.LogInformation("Url with id {Id} was deleted by user with ID: {userId}.", request.Id, _contextService.Identity!.Id);
        }
    }
}
