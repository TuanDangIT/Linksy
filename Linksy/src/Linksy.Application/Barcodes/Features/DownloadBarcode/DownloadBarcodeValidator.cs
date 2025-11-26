using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Barcodes.Features.DownloadBarcode
{
    internal class DownloadBarcodeValidator : AbstractValidator<DownloadBarcode>
    {
        public DownloadBarcodeValidator()
        {
            RuleFor(d => d.BarcodeId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
