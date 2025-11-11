using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Pagination.Exceptions
{
    internal class InvalidFilterPropertyException(IEnumerable<string> specifiedFilterProperties, IEnumerable<string> availableFilterProperties) : 
        LinksyException($"One or more specified filters: {string.Join(", ", specifiedFilterProperties)} are invalid. Please choose one of the following filters: {string.Join(", ", availableFilterProperties)}.")
    {
    }
}
