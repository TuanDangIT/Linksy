using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.UmtParameters.Features.DeleteUmtParameter
{
    internal class DeleteUmtParameterValidator : AbstractValidator<DeleteUmtParameter>
    {
        public DeleteUmtParameterValidator()
        {
            RuleFor(d => d.UmtParameterId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
