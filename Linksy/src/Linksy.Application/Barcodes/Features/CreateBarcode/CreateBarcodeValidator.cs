using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Barcodes.Features.CreateBarcode
{
    internal class CreateBarcodeValidator : AbstractValidator<CreateBarcode>
    {
        public CreateBarcodeValidator()
        {
            RuleFor(c => c.Url.OriginalUrl)
                .NotEmpty()
                .NotNull()
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("The OriginalUrl must be a valid absolute URL.");
            RuleFor(c => c.Url.CustomCode)
                .NotEmpty();
        }
    }
}
