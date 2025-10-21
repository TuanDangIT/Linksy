using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.SetActiveStatus
{
    internal class SetActiveStatusValidator : AbstractValidator<SetActiveStatus>
    {
        public SetActiveStatusValidator()
        {
            RuleFor(s => s.UrlId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(s => s.IsActive)
                .NotNull();
        }
    }
}
