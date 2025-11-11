using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Barcodes.Features.ModifyTags
{
    internal class ModifyTagsValidator : AbstractValidator<ModifyTags>
    {
        public ModifyTagsValidator()
        {
            RuleFor(m => m.BarcodeId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
