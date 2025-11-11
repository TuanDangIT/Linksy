using Linksy.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Infrastructure.Pagination.Exceptions
{
    public class InvalidFilterPropertyTypeException(string filter) : LinksyException($"Filtering is not supported for type {filter}.")
    {
    }
}
