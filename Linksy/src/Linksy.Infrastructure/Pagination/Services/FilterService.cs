using Linksy.Infrastructure.Pagination.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Pagination.Services
{
    internal class FilterService : IFilterService
    {
        private readonly string[] _dateSeparators = ["-to-", "to-", "-to"];
        public IQueryable<TEntity> ApplyFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, string filterValue)
        {
            var properties = propertyPath.Split('.');
            if (properties.Length > 1)
            {
                var navigationProperty = properties[0];
                var subPropertyName = properties[1];
                return ApplyNestedFilter(query, propertyPath, navigationProperty, subPropertyName, filterValue);
            }
            else
            {
                var property = typeof(TEntity).GetProperty(propertyPath, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidFilterPropertyTypeException(propertyPath);

                if(IsCollectionType(property.PropertyType))
                {
                    throw new InvalidFilterPropertyTypeException(propertyPath);
                }

                return property.PropertyType switch
                {
                    Type t when t.IsEnum => ApplyEnumFilter(query, propertyPath, property, null, filterValue),
                    Type t when t == typeof(DateTime) => ApplyDateFilter(query, propertyPath, property, null, filterValue),
                    Type t when t == typeof(string) => ApplyStringFilter(query, property, null, filterValue),
                    Type t when t == typeof(decimal) => ApplyDecimalFilter(query, propertyPath, property, null, filterValue),
                    Type t when t == typeof(int) => ApplyIntegerFilter(query, propertyPath, property, null, filterValue),
                    Type t when t == typeof(bool) => ApplyBooleanFilter(query, propertyPath, property, null, filterValue),
                    Type t when t == typeof(Guid) => ApplyGuidFilter(query, propertyPath, property, null, filterValue),
                    _ => throw new InvalidFilterPropertyTypeException(propertyPath)
                };
            }
        }

        private bool IsCollectionType(Type type)
            => type != typeof(string) &&
               (type.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition()) ||
               type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)));

        private Type GetCollectionElementType(Type collectionType)
        {
            if (collectionType.IsArray)
            {
                return collectionType.GetElementType()!;
            }
            return collectionType.GetGenericArguments()[0];
        }
        private IQueryable<TEntity> ApplyNestedFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, string navigationProperty, string subPropertyName, string filterValue)
        {
            var navigation = typeof(TEntity).GetProperty(navigationProperty, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ??
                throw new InvalidFilterPropertyTypeException(propertyPath);

            if (IsCollectionType(navigation.PropertyType))
            {
                Type elementType = GetCollectionElementType(navigation.PropertyType) ?? throw new InvalidFilterPropertyTypeException(propertyPath);
                var subProperty = elementType.GetProperty(subPropertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ??
                    throw new InvalidFilterPropertyTypeException(propertyPath);

                return subProperty.PropertyType switch
                {
                    Type t when t.IsEnum => ApplyEnumCollectionFilter(query, propertyPath, navigation, subProperty, filterValue),
                    Type t when t == typeof(DateTime) => ApplyDateCollectionFilter(query, propertyPath, navigation, subProperty, filterValue),
                    Type t when t == typeof(string) => ApplyStringCollectionFilter(query, navigation, subProperty, filterValue),
                    Type t when t == typeof(decimal) => ApplyDecimalCollectionFilter(query, propertyPath, navigation, subProperty, filterValue),
                    Type t when t == typeof(int) => ApplyIntegerCollectionFilter(query, propertyPath, navigation, subProperty, filterValue),
                    Type t when t == typeof(bool) => ApplyBooleanCollectionFilter(query, propertyPath, navigation, subProperty, filterValue),
                    _ => throw new InvalidFilterPropertyTypeException(propertyPath)
                };
            }
            else
            {
                var subProperty = navigation.PropertyType.GetProperty(subPropertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ??
                    throw new InvalidFilterPropertyTypeException(propertyPath);
                if (navigation == null || subProperty == null)
                {
                    throw new InvalidFilterPropertyTypeException(propertyPath);
                }

                return subProperty.PropertyType switch
                {
                    Type t when t.IsEnum => ApplyEnumFilter(query, propertyPath, navigation, subProperty, filterValue),
                    Type t when t == typeof(DateTime) => ApplyDateFilter(query, propertyPath, navigation, subProperty, filterValue),
                    Type t when t == typeof(string) => ApplyStringFilter(query, navigation, subProperty, filterValue),
                    Type t when t == typeof(decimal) => ApplyDecimalFilter(query, propertyPath, navigation, subProperty, filterValue),
                    Type t when t == typeof(int) => ApplyIntegerFilter(query, propertyPath, navigation, subProperty, filterValue),
                    Type t when t == typeof(bool) => ApplyBooleanFilter(query, propertyPath, subProperty, null, filterValue),
                    _ => throw new InvalidFilterPropertyTypeException(propertyPath)
                };
            }
        }

        #region Collection Filters with Subproperties
        private IQueryable<TEntity> ApplyStringCollectionFilter<TEntity>(IQueryable<TEntity> query, PropertyInfo collectionProperty, PropertyInfo itemProperty, string filterValue)
        {
            return query.Where(x => EF.Property<IEnumerable<object>>(x!, collectionProperty.Name)
                .Any(item => EF.Property<string>(item, itemProperty.Name).ToLower().Contains(filterValue.ToLower())));
        }

        private IQueryable<TEntity> ApplyIntegerCollectionFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, PropertyInfo collectionProperty, PropertyInfo itemProperty, string filterValue)
        {
            if (!int.TryParse(filterValue, out var parsedInt))
            {
                throw new InvalidFilterPropertyTypeException(propertyPath);
            }
            return query.Where(x => EF.Property<IEnumerable<object>>(x!, collectionProperty.Name)
                .Any(item => EF.Property<int>(item, itemProperty.Name) == parsedInt));
        }

        private IQueryable<TEntity> ApplyDecimalCollectionFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, PropertyInfo collectionProperty, PropertyInfo itemProperty, string filterValue)
        {
            if (!decimal.TryParse(filterValue, out var parsedDecimal))
            {
                throw new InvalidFilterPropertyTypeException(propertyPath);
            }
            return query.Where(x => EF.Property<IEnumerable<object>>(x!, collectionProperty.Name)
                .Any(item => EF.Property<decimal>(item, itemProperty.Name) == parsedDecimal));
        }

        private IQueryable<TEntity> ApplyGuidCollectionFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, PropertyInfo collectionProperty, PropertyInfo itemProperty, string filterValue)
        {
            if (!Guid.TryParse(filterValue, out var parsedGuid))
            {
                throw new InvalidFilterPropertyTypeException(propertyPath);
            }
            return query.Where(x => EF.Property<IEnumerable<object>>(x!, collectionProperty.Name)
                .Any(item => EF.Property<Guid>(item, itemProperty.Name) == parsedGuid));
        }

        private IQueryable<TEntity> ApplyDateCollectionFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, PropertyInfo collectionProperty, PropertyInfo itemProperty, string filterValue)
        {
            var dateRange = filterValue.Split(_dateSeparators, StringSplitOptions.None);
            var fromDate = ParseDateOrThrow(dateRange[0], propertyPath)?.Date;
            var toDate = dateRange.Length > 1 ? ParseDateOrThrow(dateRange[1], propertyPath)?.Date.AddDays(1).AddTicks(-1) : null;

            return query.Where(x => EF.Property<IEnumerable<object>>(x!, collectionProperty.Name)
                .Any(item =>
                    (!fromDate.HasValue || EF.Property<DateTime>(item, itemProperty.Name) >= fromDate.Value.ToUniversalTime()) &&
                    (!toDate.HasValue || EF.Property<DateTime>(item, itemProperty.Name) <= toDate.Value.ToUniversalTime())
                ));
        }

        private IQueryable<TEntity> ApplyEnumCollectionFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, PropertyInfo collectionProperty, PropertyInfo itemProperty, string filterValue)
        {
            if (!Enum.TryParse(itemProperty.PropertyType, filterValue, ignoreCase: true, out var enumValue))
            {
                throw new InvalidValueForEnumTypeException(propertyPath, itemProperty.PropertyType);
            }
            return query.Where(x => EF.Property<IEnumerable<object>>(x!, collectionProperty.Name)
                .Any(item => EF.Property<object>(item, itemProperty.Name).Equals(enumValue)));
        }

        private IQueryable<TEntity> ApplyBooleanCollectionFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, PropertyInfo collectionProperty, PropertyInfo itemProperty, string filterValue)
        {
            if (!bool.TryParse(filterValue, out var parsedBool))
            {
                throw new InvalidFilterPropertyTypeException(propertyPath);
            }
            return query.Where(x => EF.Property<IEnumerable<object>>(x!, collectionProperty.Name)
                .Any(item => EF.Property<bool>(item, itemProperty.Name) == parsedBool));
        }
        #endregion  
        #region Direct Filters
        private IQueryable<TEntity> ApplyEnumFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, PropertyInfo property, PropertyInfo? subProperty, string filterValue)
        {
            if (!Enum.TryParse(property.PropertyType, filterValue, ignoreCase: true, out var enumValue))
            {
                throw new InvalidValueForEnumTypeException(propertyPath, property.PropertyType);
            }
            return subProperty is null
                ? query.Where(x => EF.Property<object>(x!, property.Name).Equals(enumValue))
                : query = query.Where(x => EF.Property<object>(EF.Property<object>(x!, subProperty.Name), property.Name).Equals(enumValue));
        }

        private IQueryable<TEntity>  ApplyStringFilter<TEntity>(IQueryable<TEntity> query, PropertyInfo property, PropertyInfo? subProperty, string filterValue)
            => subProperty is null
                ? query.Where(x => EF.Property<string>(x!, property.Name).ToLower().Contains(filterValue.ToLower()))
                : query.Where(x => EF.Property<string>(EF.Property<object>(x!, property.Name), subProperty.Name).ToLower().Contains(filterValue.ToLower()));

        private IQueryable<TEntity> ApplyDecimalFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, PropertyInfo property, PropertyInfo? subProperty, string filterValue)
        {
            if (!decimal.TryParse(filterValue, out var parsedDecimal))
            {
                throw new InvalidFilterPropertyTypeException(propertyPath);
            }
            return subProperty is null
                ? query.Where(x => EF.Property<decimal>(x!, property.Name) == parsedDecimal)
                : query.Where(x => EF.Property<decimal>(EF.Property<object>(x!, property.Name), subProperty.Name) == parsedDecimal);
        }

        private IQueryable<TEntity> ApplyIntegerFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, PropertyInfo property, PropertyInfo? subProperty, string filterValue)
        {
            if (!int.TryParse(filterValue, out var parsedDecimal))
            {
                throw new InvalidFilterPropertyTypeException(propertyPath);
            }
            return subProperty is null
                ? query.Where(x => EF.Property<int>(x!, property.Name) == parsedDecimal)
                : query.Where(x => EF.Property<int>(EF.Property<object>(x!, property.Name), subProperty.Name) == parsedDecimal);
        }

        private IQueryable<TEntity> ApplyDateFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, PropertyInfo property, PropertyInfo? subProperty, string filterValue)
        {
            var dateRange = filterValue.Split(_dateSeparators, StringSplitOptions.None);
            var fromDate = ParseDateOrThrow(dateRange[0], propertyPath);
            var toDate = dateRange.Length > 1 ? ParseDateOrThrow(dateRange[1], propertyPath) : null;
            if (toDate?.TimeOfDay == TimeSpan.Zero)
            {
                toDate = toDate!.Value.AddDays(1).AddTicks(-1);
            }
            return subProperty is null
                ? query.Where(x =>
                    (!fromDate.HasValue || EF.Property<DateTime>(x!, property.Name) >= fromDate.Value.ToUniversalTime()) &&
                    (!toDate.HasValue || EF.Property<DateTime>(x!, property.Name) <= toDate.Value.ToUniversalTime()))
                : query.Where(x =>
                    (!fromDate.HasValue || EF.Property<DateTime>(EF.Property<object>(x!, property.Name), subProperty.Name) >= fromDate.Value.ToUniversalTime()) &&
                    (!toDate.HasValue || EF.Property<DateTime>(EF.Property<object>(x!, property.Name), subProperty.Name) <= toDate.Value.ToUniversalTime()));
        }

        private IQueryable<TEntity> ApplyGuidFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, PropertyInfo property, PropertyInfo? subProperty, string filterValue)
        {
            if (!Guid.TryParse(filterValue, out var parsedGuid))
            {
                throw new InvalidFilterPropertyTypeException(propertyPath);
            }
            return subProperty is null
                ? query.Where(x => EF.Property<Guid>(x!, property.Name) == parsedGuid)
                : query.Where(x => EF.Property<Guid>(EF.Property<object>(x!, property.Name), subProperty.Name) == parsedGuid);
        }

        private DateTime? ParseDateOrThrow(string? dateString, string propertyPath)
        {
            if (string.IsNullOrWhiteSpace(dateString))
            {
                return null;
            }
            return DateTime.TryParse(dateString.Trim(), null, DateTimeStyles.AssumeUniversal, out var parsedDate)
                ? parsedDate.ToUniversalTime()
                : throw new InvalidFilterPropertyTypeException(propertyPath);
        }

        private IQueryable<TEntity> ApplyBooleanFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, PropertyInfo property, PropertyInfo? subProperty, string filterValue)
        {
            if (!bool.TryParse(filterValue, out var parsedBool))
            {
                throw new InvalidFilterPropertyTypeException(propertyPath);
            }

            return subProperty is null
                ? query.Where(x => EF.Property<bool>(x!, property.Name) == parsedBool)
                : query.Where(x => EF.Property<bool>(EF.Property<object>(x!, subProperty.Name), property.Name) == parsedBool);
        }
        #endregion
    }
}
