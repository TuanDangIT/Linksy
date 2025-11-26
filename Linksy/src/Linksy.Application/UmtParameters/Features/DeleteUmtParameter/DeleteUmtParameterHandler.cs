using Linksy.Application.Abstractions;
using Linksy.Application.Shared.ScanCodes;
using Linksy.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.UmtParameters.Features.DeleteUmtParameter
{
    internal class DeleteUmtParameterHandler : ICommandHandler<DeleteUmtParameter>
    {
        private readonly IUmtParameterRepository _umtParameterRepository;
        private readonly IScanCodeService _scanCodeService;
        private readonly IContextService _contextService;
        private readonly ILogger<DeleteUmtParameterHandler> _logger;

        public DeleteUmtParameterHandler(IUmtParameterRepository umtParameterRepository, IScanCodeService scanCodeService, 
            IContextService contextService, ILogger<DeleteUmtParameterHandler> logger)
        {
            _umtParameterRepository = umtParameterRepository;
            _scanCodeService = scanCodeService;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task Handle(DeleteUmtParameter request, CancellationToken cancellationToken)
        {
            var umtParameter = await _umtParameterRepository.GetByIdAsync(request.UmtParameterId, cancellationToken, u => u.QrCode);
            if(umtParameter is null)
            {
                return;
            }
            if(umtParameter.QrCode is not null)
            {
                await _scanCodeService.DeleteAsync(umtParameter.QrCode.ScanCodeImage.FileName, _contextService.Identity!.Id, cancellationToken);
            }
            await _umtParameterRepository.DeleteAsync(request.UmtParameterId);
            _logger.LogInformation("UMT Parameter with ID: {UmtParameterId} was deleted by user with ID {UserId}.",
                request.UmtParameterId, _contextService.Identity!.Id);
        }
    }
}
