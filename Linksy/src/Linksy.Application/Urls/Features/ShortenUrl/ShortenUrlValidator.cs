using FluentValidation;
using Linksy.Application.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.ShortenUrl
{
    internal class ShortenUrlValidator : AbstractValidator<ShortenUrl>
    {
        public ShortenUrlValidator()
        {
            RuleFor(s => s.OriginalUrl)
                .NotEmpty()
                .NotNull()
                .Must(IsAValidUrl)
                .WithMessage("The OriginalUrl must be a valid absolute URL.")
                .MaximumLength(512);
            RuleFor(s => s.CustomCode)
                .MinimumLength(1)
                .MaximumLength(128)
                .Matches(@"^\S*$") 
                .WithMessage("CustomCode cannot contain spaces.");
            RuleForEach(s => s.UmtParameters)
                    .ChildRules(umtParam =>
                    {
                        umtParam.RuleFor(u => u.UmtSource)
                            .MaximumLength(128)
                            .WithMessage("UmtSource cannot exceed 128 characters.")
                            .NotNull();

                        umtParam.RuleFor(u => u.UmtMedium)
                            .MaximumLength(128)
                            .WithMessage("UmtMedium cannot exceed 128 characters.")
                            .NotNull();

                        umtParam.RuleFor(u => u.UmtCampaign)
                            .MaximumLength(128)
                            .WithMessage("UmtCampaign cannot exceed 128 characters.")
                            .NotNull();

                        umtParam.RuleFor(u => u)
                            .Must(HaveAtLeastOneUtmParameter)
                            .WithMessage("At least one UTM parameter (Source, Medium, or Campaign) must be provided.");
                    });
        }

        private bool IsAValidUrl(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                return false;

            return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
        }

        private bool HaveAtLeastOneUtmParameter(UmtParameterDto dto)
        {
            return !string.IsNullOrWhiteSpace(dto.UmtSource) ||
                   !string.IsNullOrWhiteSpace(dto.UmtMedium) ||
                   !string.IsNullOrWhiteSpace(dto.UmtCampaign);
        }
    }
}
