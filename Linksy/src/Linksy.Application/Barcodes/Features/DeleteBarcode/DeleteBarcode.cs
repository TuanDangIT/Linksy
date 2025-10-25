using Linksy.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Barcodes.Features.DeleteBarcode
{
    public record class DeleteBarcode(int BarcodeId, bool IncludeUrlInDeletion) : ICommand;
}
