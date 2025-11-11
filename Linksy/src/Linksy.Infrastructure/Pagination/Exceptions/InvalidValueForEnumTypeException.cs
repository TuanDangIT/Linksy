using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Pagination.Exceptions
{
    internal class InvalidValueForEnumTypeException(string filter, Type enumType) : LinksyException($"The provided value: {filter} is not a valid member of the enum: {enumType.Name}.")
    {
    }
}
