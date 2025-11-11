using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.QrCodes.Features.DeleteQrCode
{
    internal class DeleteQrCodeValidator : AbstractValidator<DeleteQrCode>
    {
        public DeleteQrCodeValidator()
        {
            RuleFor(m => m.QrCodeId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
