using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Barcodes.Features.DeleteBarcode
{
    internal class DeleteBarcodeValidator : AbstractValidator<DeleteBarcode>
    {
        public DeleteBarcodeValidator()
        {
            RuleFor(d => d.BarcodeId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
