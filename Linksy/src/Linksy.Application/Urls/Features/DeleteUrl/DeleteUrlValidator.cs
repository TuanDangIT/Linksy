using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.DeleteUrl
{
    internal class DeleteUrlValidator : AbstractValidator<DeleteUrl>
    {
        public DeleteUrlValidator()
        {
            RuleFor(d => d.Id)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
