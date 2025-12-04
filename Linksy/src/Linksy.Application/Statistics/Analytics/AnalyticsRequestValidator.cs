using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Statistics.Analytics
{
    internal class AnalyticsRequestValidator : AbstractValidator<AnalyticsRequest>
    {
        private static readonly IEnumerable<string> _availableTimeIntervals = Enum.GetNames<TimeInterval>();
        private static readonly IEnumerable<string> _availableTimeRanges = Enum.GetNames<TimeRange>();
        //private static readonly Dictionary<string, List<string>> _allowedIntervals = new()
        //{
        //    { "day", new List<string> { "minutes30", "hourly" } },
        //    { "week", new List<string> { "hourly", "daily" } },
        //    { "month", new List<string> { "daily", "weekly" } },
        //    { "year", new List<string> { "weekly", "monthly" } },
        //    { "currentyear", new List<string> { "weekly", "monthly" } },
        //    { "custom", new List<string> { "minutes30", "hourly", "daily", "weekly", "monthly" } }
        //};

        //Add more validation rules that are used in AnalyticsService here in the future.
        public AnalyticsRequestValidator()
        {
            RuleFor(a => a.TimeRange)
                .NotNull()
                .IsEnumName(typeof(TimeRange), caseSensitive: false)
                .WithMessage(v => $"Invalid time range: {v.TimeRange}. Allowed values: {string.Join(", ", _availableTimeRanges)}");

            RuleFor(a => a.Interval)
                .NotNull()
                .IsEnumName(typeof(TimeInterval), caseSensitive: false)
                .WithMessage(v => $"Invalid time interval: {v.Interval}. Allowed values: {string.Join(", ", _availableTimeIntervals)}");

            RuleFor(a => a.StartDate)
                .NotEmpty()
                .When(a => a.TimeRange != null && a.TimeRange.Equals(TimeRange.Custom.ToString(), StringComparison.OrdinalIgnoreCase))
                .When(a => a.TimeRange.Equals(TimeRange.Custom.ToString(), StringComparison.OrdinalIgnoreCase))
                .WithMessage("StartDate is required when TimeRange is 'custom'");

            RuleFor(a => a.EndDate)
                .NotEmpty()
                .When(a => a.TimeRange != null && a.TimeRange.Equals(TimeRange.Custom.ToString(), StringComparison.OrdinalIgnoreCase))
                .When(a => a.TimeRange.Equals(TimeRange.Custom.ToString(), StringComparison.OrdinalIgnoreCase))
                .WithMessage("EndDate is required when TimeRange is 'custom'");

            RuleFor(a => a.StartDate)
                .Null()
                .When(a => !a.TimeRange.Equals(TimeRange.Custom.ToString(), StringComparison.OrdinalIgnoreCase))
                .WithMessage("StartDate must be null when TimeRange is not 'custom'");

            RuleFor(a => a.EndDate)
               .Null()
               .When(a => !a.TimeRange.Equals(TimeRange.Custom.ToString(), StringComparison.OrdinalIgnoreCase))
               .WithMessage("EndDate must be null when TimeRange is not 'custom'");

            RuleFor(a => a.EndDate)
                .GreaterThanOrEqualTo(a => a.StartDate)
                .When(a => a.StartDate.HasValue && a.EndDate.HasValue)
                .WithMessage("EndDate must be greater than or equal to StartDate");

            RuleFor(a => a.StartDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .When(a => a.StartDate.HasValue)
                .WithMessage("StartDate cannot be in the future");

            RuleFor(a => a.EndDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .When(a => a.EndDate.HasValue)
                .WithMessage("EndDate cannot be in the future");
        }
    }
}
