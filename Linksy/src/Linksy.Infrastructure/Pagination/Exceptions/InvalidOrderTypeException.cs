using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Pagination.Exceptions
{
    internal class InvalidOrderTypeException(string specifiedOrderType, IEnumerable<string> availableOrderTypes) : 
        Exception($"The specified order type: {specifiedOrderType} is invalid. Please choose one of the following order types: {string.Join(", ", availableOrderTypes)}.")
    {
    }
}
