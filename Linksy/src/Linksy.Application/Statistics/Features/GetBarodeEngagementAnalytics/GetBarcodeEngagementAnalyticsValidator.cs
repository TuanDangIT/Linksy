using FluentValidation;
using Linksy.Application.Statistics.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Statistics.Features.GetBarodeEngagementAnalytics
{
    internal class GetBarcodeEngagementAnalyticsValidator : AbstractValidator<GetBarcodeEngagementAnalytics>
    {
        public GetBarcodeEngagementAnalyticsValidator()
        {
            Include(new AnalyticsRequestValidator());
        }
    }
}
