using Linksy.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.QrCodes.Features.ModifyTags
{
    public record class ModifyTags(int QrCodeId, IEnumerable<string> Tags) : ICommand;
}
