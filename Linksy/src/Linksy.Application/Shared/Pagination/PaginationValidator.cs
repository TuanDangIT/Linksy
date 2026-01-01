using FluentValidation;
using Linksy.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Shared.Pagination
{
    public abstract class PaginationValidator<TQuery, TEntity> : AbstractValidator<TQuery>
        where TQuery : IPaginatedQuery
    {
        private static readonly HashSet<string> _allowedSortTypes = new(StringComparer.OrdinalIgnoreCase) { "asc", "desc" };
        protected readonly IPaginationConfiguration<TEntity> PaginationConfig;

        protected PaginationValidator(IPaginationConfiguration<TEntity> paginationConfig)
        {
            PaginationConfig = paginationConfig;

            RuleFor(q => q.PageNumber)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(q => q.PageSize)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0).WithMessage("Page size must be greater than 0.");

            RuleFor(q => q.Sort)
                .Custom((sort, context) =>
                {
                    if (string.IsNullOrWhiteSpace(sort))
                        return;

                    var sortExpressions = sort.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    var invalidProperties = new List<string>();

                    foreach (var sortExpression in sortExpressions)
                    {
                        var sortParts = sortExpression.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                        var sortProperty = sortParts[0].Trim();

                        if (!PaginationConfig.AllowedOrders.Keys.Any(k => k.Equals(sortProperty, StringComparison.OrdinalIgnoreCase)))
                        {
                            invalidProperties.Add(sortProperty);
                            continue;
                        }

                        if (sortParts.Length > 1)
                        {
                            var sortType = sortParts[1].Trim();
                            if (!_allowedSortTypes.Contains(sortType))
                            {
                                context.AddFailure("Sort",
                                    $"Sort type '{sortType}' is invalid. Allowed sort types are: asc, desc.");
                            }
                        }
                    }

                    if (invalidProperties.Any())
                    {
                        var availableOrders = string.Join(", ", PaginationConfig.AllowedOrders.Keys);
                        context.AddFailure("Sort",
                            $"One or more specified sorts: {string.Join(", ", invalidProperties)} are invalid. Please choose one of the following sorts: {availableOrders}.");
                    }
                });

            RuleFor(q => q.Filters)
                .Custom((filters, context) =>
                {
                    if (filters == null || filters.Count == 0)
                        return;

                    var invalidFilters = filters.Keys
                        .Where(key => !PaginationConfig.AllowedFilters.Contains(key))
                        .ToList();

                    if (invalidFilters.Any())
                    {
                        var availableFilters = string.Join(", ", PaginationConfig.AllowedFilters);
                        context.AddFailure("Filters",
                            $"One or more specified filters: {string.Join(", ", invalidFilters)} are invalid. Please choose one of the following filters: {availableFilters}.");
                    }
                });
        }
    }
}
