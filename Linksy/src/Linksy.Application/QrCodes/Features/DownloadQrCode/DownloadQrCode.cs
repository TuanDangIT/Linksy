using Linksy.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.QrCodes.Features.DownloadQrCode
{
    public record class DownloadQrCode(int QrCodeId) : IQuery<FileStreamResult>;
}
