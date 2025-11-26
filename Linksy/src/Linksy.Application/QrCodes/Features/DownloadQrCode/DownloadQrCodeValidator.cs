using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.QrCodes.Features.DownloadQrCode
{
    internal class DownloadQrCodeValidator : AbstractValidator<DownloadQrCode>
    {
        public DownloadQrCodeValidator()
        {
            RuleFor(d => d.QrCodeId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
