using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Barcodes.Features.GetBarcode
{
    internal class GetBarcodeValidator : AbstractValidator<GetBarcode> 
    {
        public GetBarcodeValidator()
        {
            RuleFor(x => x.BarcodeId)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0);
        }
    }
}
