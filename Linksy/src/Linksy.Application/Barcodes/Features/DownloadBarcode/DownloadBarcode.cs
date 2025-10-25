using Linksy.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Barcodes.Features.DownloadBarcode
{
    public record class DownloadBarcode(int BarcodeId) : IQuery<FileStreamResult>;
}
