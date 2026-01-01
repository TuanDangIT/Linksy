using Linksy.Application.Shared.Pagination;
using Linksy.Infrastructure.Pagination.Configuration;
using Linksy.Infrastructure.Pagination.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Pagination.Services
{
    internal class PaginationService<TEntity> : IPaginationService<TEntity>
        where TEntity : class
    {
        private readonly IFilterService _filterService;
        private readonly Dictionary<string, bool> _orderTypes = new()
        {
            { "asc", false },
            { "desc", true }
        };
        private readonly Dictionary<string, Expression<Func<TEntity, object>>>? _allowedOrders;
        private readonly HashSet<string>? _allowedFilters;
        private const char _orderSeparator = ':';

        public PaginationService(IFilterService filterService, IPaginationConfiguration<TEntity> paginationConfiguration)
        {
            _filterService = filterService;
            _allowedOrders = paginationConfiguration.AllowedOrders;
            _allowedFilters = paginationConfiguration.AllowedFilters;
        }

        public async Task<PagedResult<TDto>> PaginateAsync<TDto>(
            IQueryable<TEntity> query,
            int pageNumber,
            int pageSize,
            Dictionary<string, string>? filters,
            IEnumerable<string>? orders,
            Expression<Func<TEntity, TDto>> selector,
            CancellationToken cancellationToken = default) /*where TEntity : class*/
        {
            if (filters is not null && _allowedFilters is not null && filters.Any(f => !_allowedFilters.Contains(f.Key)))
            {
                var wrongFilters = filters.Keys.Where(f => !_allowedFilters.Contains(f)).ToList();
                throw new InvalidFilterPropertyException(wrongFilters, [.. _allowedFilters]);
            }

            if (orders is not null && _allowedOrders is not null && orders.Any(o => !_allowedOrders.ContainsKey(o.Split(_orderSeparator)[0])))
            {
                var wrongOrders = orders.Where(o => !_allowedOrders.ContainsKey(o.Split(_orderSeparator)[0])).ToList();
                throw new InvalidOrderByPropertyException(wrongOrders, [.. _allowedOrders.Keys]);
            }

            if (filters is not null && filters.Count != 0)
            {
                foreach (var filter in filters)
                {
                    query = _filterService.ApplyFilter(query, filter.Key, filter.Value);
                }
            }

            if (orders is not null && orders.Any() && _allowedOrders is not null)
            {
                bool isFirstOrder = true;
                foreach (var order in orders)
                {
                    var orderParts = order.Split(_orderSeparator, StringSplitOptions.RemoveEmptyEntries);

                    bool sortByDescending = false;
                    if (orderParts.Length > 1)
                    {
                        var orderType = orderParts[1].ToLower();
                        if (!_orderTypes.TryGetValue(orderType, out sortByDescending))
                        {
                            throw new InvalidOrderTypeException(orderType, [.. _orderTypes.Keys]);
                        }
                    }

                    if (isFirstOrder)
                    {
                        query = sortByDescending
                            ? query.OrderByDescending(_allowedOrders[orderParts[0]])
                            : query.OrderBy(_allowedOrders[orderParts[0]]);
                        isFirstOrder = false;
                    }
                    else
                    {
                        var orderedQuery = (IOrderedQueryable<TEntity>)query;
                        query = sortByDescending
                            ? orderedQuery.ThenByDescending(_allowedOrders[orderParts[0]])
                            : orderedQuery.ThenBy(_allowedOrders[orderParts[0]]);
                    }
                }
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .Select(selector)
                .ToListAsync(cancellationToken);

            return new PagedResult<TDto>(items, totalCount, pageSize, pageNumber);
        }
    }
}

