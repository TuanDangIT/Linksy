using Linksy.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Barcodes.Features.ModifyTags
{
    public record class ModifyTags(int BarcodeId, IEnumerable<string> Tags) : ICommand;
}
