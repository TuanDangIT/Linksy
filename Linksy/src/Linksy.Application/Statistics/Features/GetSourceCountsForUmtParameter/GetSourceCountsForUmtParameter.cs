using Linksy.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Statistics.Features.GetSourceCountsForUmtParameter
{
    public record class GetSourceCountsForUmtParameter(int UrlId) : IQuery<GetSourceCountsForUmtParameterResponse>;
}
