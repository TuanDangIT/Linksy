using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.AddBarcode
{
    internal class AddBarcodeValidator : AbstractValidator<AddBarcode>
    {
        public AddBarcodeValidator()
        {
            RuleFor(a => a.UrlId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
