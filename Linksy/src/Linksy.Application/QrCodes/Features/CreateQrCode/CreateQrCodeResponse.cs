using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Application.QrCodes.Features.CreateQrCode
{
    public record class CreateQrCodeResponse(int QrCodeId, int UrlId, string ImageUrlPath, string FileName);
}
