using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Barcodes.Features.BrowseBarcodes
{
    internal class BrowseBarcodesValidator : AbstractValidator<BrowseBarcodes>
    {
        public BrowseBarcodesValidator()
        {
            RuleFor(b => b.PageNumber)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");
            RuleFor(b => b.PageSize)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0).WithMessage("Page size must be greater than 0.");
        }
    }
}
