using Linksy.Application.Shared.Configuration;
using Linksy.Application.Statistics.Analytics;
using Linksy.Domain.Entities.Tracking;
using Linksy.Infrastructure.Analytics.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Statistics
{
    internal class AnalyticsService : IAnalyticsService
    {
        private readonly TimeProvider _timeProvider;
        private readonly LinksyConfig _linksyConfig;
        private readonly ILogger<AnalyticsService> _logger;
        private static readonly Dictionary<string, List<string>> _allowedIntervals = new()
        {
            { "day", new List<string> { "minutes30", "hourly" } },
            { "week", new List<string> { "hourly", "daily" } },
            { "month", new List<string> { "daily", "weekly" } },
            { "year", new List<string> { "weekly", "monthly" } },
            { "currentyear", new List<string> { "weekly", "monthly" } },
            { "custom", new List<string> { "minutes30", "hourly", "daily", "weekly", "monthly" } }
        };

        public AnalyticsService(TimeProvider timeProvider, LinksyConfig linksyConfig, ILogger<AnalyticsService> logger)
        {
            _timeProvider = timeProvider;
            _linksyConfig = linksyConfig;
            _logger = logger;
        }
        public async Task<AnalyticsResponse> GetAnalyticsAsync<T>(IQueryable<T> query, AnalyticsRequest request, CancellationToken cancellationToken = default) 
            where T : Engagement
        {
            ValidateRequest(request);
            if (!Enum.TryParse<TimeInterval>(request.Interval, true, out var interval))
            {
                throw new InvalidTimeIntervalException(request.Interval);
            }
            var (startDate, endDate) = GetDateRange(request);
            query = query.Where(e => e.EngagedAt >= startDate && e.EngagedAt <= endDate);
            var generatedBuckets = GenerateTimeBuckets(startDate, endDate, interval);
            var grouped = interval switch
            {
                TimeInterval.Minutes30 => await query
                    .GroupBy(e => new
                    {
                        e.EngagedAt.Year,
                        e.EngagedAt.Month,
                        e.EngagedAt.Day,
                        e.EngagedAt.Hour,
                        HalfHour = e.EngagedAt.Minute / 30
                    })
                    .Select(g => new
                    {
                        PeriodStart = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day, g.Key.Hour, g.Key.HalfHour * 30, 0),
                        Count = g.Count(),
                        UniqueIpCount = g.Select(x => x.IpAddress).Distinct().Count()
                    })
                    .OrderBy(g => g.PeriodStart)
                    .Select(g => new DataPoint(g.PeriodStart, g.Count, g.UniqueIpCount))
                    .ToListAsync(cancellationToken),

                TimeInterval.Hourly => await query
                    .GroupBy(e => new
                    {
                        e.EngagedAt.Year,
                        e.EngagedAt.Month,
                        e.EngagedAt.Day,
                        e.EngagedAt.Hour
                    })
                    .Select(g => new
                    {
                        PeriodStart = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day, g.Key.Hour, 0, 0),
                        Count = g.Count(),
                        UniqueIpCount = g.Select(x => x.IpAddress).Distinct().Count()
                    })
                    .OrderBy(g => g.PeriodStart)
                    .Select(g => new DataPoint(g.PeriodStart, g.Count, g.UniqueIpCount))
                    .ToListAsync(cancellationToken),

                TimeInterval.Daily => await query
                    .GroupBy(e => e.EngagedAt.Date)
                    .Select(g => new
                    {
                        PeriodStart = g.Key,
                        Count = g.Count(),
                        UniqueIpCount = g.Select(x => x.IpAddress).Distinct().Count()
                    })
                    .OrderBy(g => g.PeriodStart)
                    .Select(g => new DataPoint(g.PeriodStart, g.Count, g.UniqueIpCount))
                    .ToListAsync(cancellationToken),

                TimeInterval.Weekly => await query
                    .GroupBy(e => new
                    {
                        e.EngagedAt.Year,
                        e.EngagedAt.Month,
                        e.EngagedAt.Day
                    })
                    .Select(g => new
                    {
                        Date = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day),
                        Count = g.Count(),
                        UniqueIpCount = g.Select(x => x.IpAddress).Distinct().Count()
                    })
                    .ToListAsync(cancellationToken)
                    .ContinueWith(task =>
                    {
                        return task.Result
                            .GroupBy(x => x.Date.AddDays(-(int)x.Date.DayOfWeek))
                            .Select(g => new DataPoint(g.Key, g.Sum(x => x.Count), g.Sum(x => x.UniqueIpCount)))
                            .OrderBy(g => g.PeriodStart)
                            .ToList();
                    }, cancellationToken),

                TimeInterval.Monthly => await query
                    .GroupBy(e => new
                    {
                        e.EngagedAt.Year,
                        e.EngagedAt.Month
                    })
                    .Select(g => new
                    {
                        PeriodStart = new DateTime(g.Key.Year, g.Key.Month, 1),
                        Count = g.Count(),
                        UniqueIpCount = g.Select(x => x.IpAddress).Distinct().Count()
                    })
                    .OrderBy(g => g.PeriodStart)
                    .Select(g => new DataPoint(g.PeriodStart, g.Count, g.UniqueIpCount))
                    .ToListAsync(cancellationToken),

                //TimeInterval.Quarterly => await query
                //    .GroupBy(e => new
                //    {
                //        e.EngagedAt.Year,
                //        Quarter = (e.EngagedAt.Month - 1) / 3
                //    })
                //    .Select(g => new
                //    {
                //        PeriodStart = new DateTime(g.Key.Year, g.Key.Quarter * 3 + 1, 1),
                //        Count = g.Count(),
                //        UniqueIpCount = g.Select(x => x.IpAddress).Distinct().Count()
                //    })
                //    .OrderBy(g => g.PeriodStart)
                //    .Select(g => new DataPoint(g.PeriodStart, g.Count, g.UniqueIpCount))
                //    .ToListAsync(cancellationToken),

                _ => throw new InvalidTimeIntervalException(interval.ToString())
            };

            var dataPoints = generatedBuckets.Select(b =>
            {
                var matchingGroup = grouped.FirstOrDefault(g => g.PeriodStart == b);
                var a = new DataPoint(b, matchingGroup?.Count ?? 0, matchingGroup?.UniqueIpCount ?? 0);
                var c = b;
                return new DataPoint(b, matchingGroup?.Count ?? 0, matchingGroup?.UniqueIpCount ?? 0);
            });
            var total = dataPoints.Sum(d => d.Count);
            var totalUniqueIps = dataPoints.Sum(d => d.UniqueIpCount);
            var averageCountPerInterval = dataPoints.Average(d => d.Count);
            var peakCount = dataPoints.Max(d => d.Count);
            var summary = new AnalyticsSummary(total, totalUniqueIps, averageCountPerInterval, peakCount);
            _logger.LogDebug("Generated analytics from {StartDate} to {EndDate} with interval {Interval}.", startDate, endDate, interval);
            return new AnalyticsResponse(request.TimeRange.ToString(), request.Interval.ToString(), startDate, endDate, dataPoints, summary);
        }
        private List<DateTime> GenerateTimeBuckets(
            DateTime start,
            DateTime end,
            TimeInterval interval)
        {
            var buckets = new List<DateTime>();
            var currentStart = start;

            while (currentStart < end)
            {
                var currentEnd = CalculatePeriodEnd(currentStart, interval);
                buckets.Add(currentStart);
                currentStart = currentEnd;
            }

            return buckets;
        }
        private DateTime CalculatePeriodEnd(DateTime periodStart, TimeInterval interval)
        {
            return interval switch
            {
                TimeInterval.Minutes30 => periodStart.AddMinutes(30),
                TimeInterval.Hourly => periodStart.AddHours(1),
                TimeInterval.Daily => periodStart.AddDays(1),
                TimeInterval.Weekly => periodStart.AddDays(7),
                TimeInterval.Monthly => periodStart.AddMonths(1),
                TimeInterval.Quarterly => periodStart.AddMonths(3),
                _ => periodStart.AddDays(1)
            };
        }
        private (DateTime startDate, DateTime endDate) GetDateRange(AnalyticsRequest request)
        {
            if (!Enum.TryParse(typeof(TimeRange), request.TimeRange, true, out var timeRange))
            {
                throw new InvalidTimeRangeException(request.TimeRange);
            }
            var now = _timeProvider.GetUtcNow().UtcDateTime;

            return timeRange switch
            {
                TimeRange.Day => (now.Date, now), 
                TimeRange.Week => (now.Date.AddDays(-7), now), 
                TimeRange.Month => (now.Date.AddDays(-30), now), 
                TimeRange.Year => (now.Date.AddDays(-365), now), 
                TimeRange.CurrentYear => (new DateTime(now.Year, 1, 1), now),
                TimeRange.Custom => (
                    DateTime.SpecifyKind(request.StartDate!.Value, DateTimeKind.Utc),
                    DateTime.SpecifyKind(request.EndDate!.Value, DateTimeKind.Utc)
                ),
                _ => throw new ArgumentException("Invalid time range.")
            };
        }

        private void ValidateRequest(AnalyticsRequest request)
        {
            if (!Enum.TryParse(typeof(TimeRange), request.TimeRange, true, out var timeRange))
            {
                throw new InvalidTimeRangeException(request.TimeRange);
            }

            if (!Enum.TryParse<TimeInterval>(request.Interval, true, out _))
            {
                throw new InvalidTimeIntervalException(request.Interval);
            }

            if (timeRange is TimeRange.Custom)
            {
                if (!request.StartDate.HasValue)
                {
                    throw new InvalidStartDateException("Start date cannot be null when time range is custom.");
                }

                if (!request.EndDate.HasValue)
                {
                    throw new InvalidStartDateException("End date cannot be null when time range is custom.");
                }

                var daysDifference = (request.EndDate.Value - request.StartDate.Value).TotalDays;
                if (daysDifference > _linksyConfig.Analytics.MaxCustomRangeDays)
                {
                    throw new ExceededTimeRangeException(_linksyConfig.Analytics.MaxCustomRangeDays);
                }

                var now = _timeProvider.GetUtcNow().UtcDateTime;
                if (request.EndDate.Value > now)
                {
                    throw new InvalidEndDateException("End date cannot be in the future.");
                }

                var minAllowedDate = now.AddYears((-1) * _linksyConfig.Analytics.MinStartDateInYears);
                if (request.StartDate.Value < minAllowedDate)
                {
                    throw new InvalidStartDateException($"Start date is earlier than the minimum allowed date: {minAllowedDate}.");
                }

                ValidateCustomRangeInterval(daysDifference, request.Interval);
            }
            else
            {
                var timeRangeLower = request.TimeRange.ToLowerInvariant();
                var intervalLower = request.Interval.ToLowerInvariant();

                if (!_allowedIntervals.TryGetValue(timeRangeLower, out var allowedIntervals))
                {
                    throw new InvalidTimeRangeException(request.TimeRange);
                }

                if (!allowedIntervals.Contains(intervalLower))
                {
                    throw new InvalidTimeIntervalForSpecifiedCustomRangeException(request.Interval, string.Join(", ", allowedIntervals));
                }
            }
        }

        private void ValidateCustomRangeInterval(double daysDifference, string interval)
        {
            var intervalLower = interval.ToLowerInvariant();
            List<string> allowedIntervals = [];

            if (daysDifference <= 1)
            {
                allowedIntervals = _allowedIntervals["day"] ?? ["minutes30", "hourly"];
            }
            else if (daysDifference <= 7)
            {
                allowedIntervals = _allowedIntervals["week"] ?? ["hourly", "daily"];
            }
            else if (daysDifference <= 30)
            {
                allowedIntervals = _allowedIntervals["month"] ?? ["daily", "weekly"];
            }
            else if (daysDifference <= 90)
            {
                allowedIntervals = _allowedIntervals["year"] ?? ["weekly", "monthly"];
            }

            if (!allowedIntervals.Contains(intervalLower))
            {
                throw new InvalidTimeIntervalForSpecifiedCustomRangeException(interval, string.Join(", ", allowedIntervals));
            }
        }
    }
}
