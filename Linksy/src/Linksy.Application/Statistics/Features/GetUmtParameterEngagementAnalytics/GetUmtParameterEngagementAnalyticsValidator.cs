using FluentValidation;
using Linksy.Application.Statistics.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Statistics.Features.GetUmtParameterEngagementAnalytics
{
    internal class GetUmtParameterEngagementAnalyticsValidator : AbstractValidator<GetUmtParameterEngagementAnalytics>
    {
        public GetUmtParameterEngagementAnalyticsValidator()
        {
            Include(new AnalyticsRequestValidator());
        }
    }
}
