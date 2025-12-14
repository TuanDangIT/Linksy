using Linksy.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.UmtParameters.Features.GetUmtParameter
{
    public record class GetUmtParameter(int UrlId, int UmtParameterId) : IQuery<GetUmtParameterResponse?>;
}
