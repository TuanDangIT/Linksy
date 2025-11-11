using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Pagination.Exceptions
{
    internal class InvalidOrderByPropertyException(IEnumerable<string> specifiedOrderProperties, IEnumerable<string> availableOrderProperties) : 
        LinksyException($"One or more specified order by properties: {string.Join(", ", specifiedOrderProperties)} are invalid. Please choose one of the following properties: {string.Join(", ", availableOrderProperties)}.")
    {
    }
}
