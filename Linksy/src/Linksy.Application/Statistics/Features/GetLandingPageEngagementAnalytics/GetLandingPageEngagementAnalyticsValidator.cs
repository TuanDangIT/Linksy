using FluentValidation;
using Linksy.Application.Statistics.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Statistics.Features.GetLandingPageEngagementAnalytics
{
    internal class GetLandingPageEngagementAnalyticsValidator : AbstractValidator<GetLandingPageEngagementAnalytics>
    {
        public GetLandingPageEngagementAnalyticsValidator()
        {
            Include(new AnalyticsRequestValidator());
        }
    }
}
