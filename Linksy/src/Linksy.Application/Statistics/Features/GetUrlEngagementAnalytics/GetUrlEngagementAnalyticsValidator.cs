using FluentValidation;
using Linksy.Application.Statistics.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Statistics.Features.GetUrlEngagementAnalytics
{
    internal class GetUrlEngagementAnalyticsValidator : AbstractValidator<GetUrlEngagementAnalytics>
    {
        public GetUrlEngagementAnalyticsValidator()
        {
            Include(new AnalyticsRequestValidator());
        }
    }
}
