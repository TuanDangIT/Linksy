using Linksy.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.UmtParameters.Features.DeleteUmtParameter
{
    public record class DeleteUmtParameter(int UmtParameterId) : ICommand;
}
