using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.Urls.Features.AddQrCode
{
    public record class AddQrCodeResponse(int QrCodeId, string ImageUrlPath, string FileName);
}
